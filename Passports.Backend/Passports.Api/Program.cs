using System.Configuration;
using NLog.Extensions.Logging;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData;
using Passports.Api.Models.LoadData.Interfaces;
using Passports.Api.Models.LoadData.Interfaces.Storages;
using Passports.Api.Models.LoadData.Loaders;
using Passports.Api.Models.Passport.Interfaces;
using Passports.Api.Services;
using Quartz;

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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if(mode == Modes.Postgres)
    app.UsePostgres();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();