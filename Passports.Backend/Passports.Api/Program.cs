using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData;
using Passports.Api.Models.LoadData.Interfaces;
using Passports.Api.Models.LoadData.Interfaces.Storages;
using Passports.Api.Models.LoadData.Loaders;
using Passports.Api.Models.Passport.Interfaces;
using Passports.Api.Services;
using Passports.Api.Services.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Passports.Api.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var nlogLoggerProvider = new NLogLoggerProvider();
// Create an ILogger.
var logger = nlogLoggerProvider.CreateLogger(typeof(Program).FullName);

builder.Services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
builder.Services.AddLogger(builder.Environment);
builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));

builder.Services.AddSingleton<IMvdFileLoader, MvdFileLoader>();
builder.Services.AddSingleton<IPassportPreparation, PassportPreparation>();

builder.Services.AddSingleton<IMvdWebClient, MvdWebClient>();
builder.Services.AddSingleton<ITemporaryPath, TemporaryPath>();
builder.Services.AddSingleton<IRepositoryPath, RepositoryPath>();
builder.Services.AddSingleton<IFileNameRecipient, FileNameRecipient>();
builder.Services.AddSingleton<IArchiver, Archiver>();
builder.Services.AddSingleton<IPassportParser, PassportParser>();

Enum.TryParse(builder.Configuration["Mode"], out Modes mode);
switch (mode)
{
    case Modes.Postgres:
        builder.Services
            .AddPostgres(builder.Configuration, builder.Environment, logger)
            .AddScoped<IDbStorage, Passports.Api.Models.LoadData.Storages.DbStorageContext>()
            .AddScoped<IPassportProvider, Passports.Api.Models.Passport.PassportProviders.DbStorageProvider>()
            .AddScoped<ILoader, DbLoader>();
        break;
    case Modes.FileStorage:
        builder.Services
            .AddSingleton<IFileStorage, Passports.Api.Models.LoadData.Storages.FileStorage>()
            .AddScoped<IPassportProvider, Passports.Api.Models.Passport.PassportProviders.FileStorageProvider>()
            .AddSingleton<ILoader, FileStorageLoader>();
        break;
    case Modes.Redis:
        builder.Services
            .AddSingleton<IRedisStorage, Passports.Api.Models.LoadData.Storages.RedisStorage>()
            .AddScoped<IPassportProvider, Passports.Api.Models.Passport.PassportProviders.RedisStorageProvider>()
            .AddSingleton<ILoader, RedisLoader>();
        break;
}

builder.Services.AddQuartz<LoadDataJob>(builder.Configuration);

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(IDbStorage).Assembly));
});

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddAuthentication(config =>
    {
        config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer("Bearer", options =>
    {
        //options.Authority = "https://localhost:6007/";
        options.Authority = "https://localhost:7061/";
        options.Audience = "PassportsWebAPI";
        options.RequireHttpsMetadata = false;
    });



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning();

builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if(mode == Modes.Postgres)
    app.UsePostgres();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            config.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
            config.RoutePrefix = string.Empty;
        }
    });
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseApiVersioning();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();