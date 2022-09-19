using System.Configuration;
using NLog.Extensions.Logging;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData;
using Passports.Api.Models.LoadData.Interfaces;
using Passports.Api.Models.LoadData.Interfaces.Storages;
using Passports.Api.Models.Passport.Interfaces;
using Passports.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var nlogLoggerProvider = new NLogLoggerProvider();
// Create an ILogger.
var logger = nlogLoggerProvider.CreateLogger(typeof(Program).FullName);

builder.Services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
builder.Services.AddLogger(builder.Environment);
builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));
Enum.TryParse(builder.Configuration["Mode"], out Modes mode);
switch (mode)
{
    case Modes.Database:
        builder.Services.AddPostgres(builder.Configuration, builder.Environment, logger);

        builder.Services.AddScoped<IDbStorage, Passports.Api.Models.LoadData.Storages.DbStorage>();
        builder.Services.AddScoped<IPassportProvider, Passports.Api.Models.Passport.PassportProviders.DbStorage>();
        break;
    case Modes.FileStorage:
        builder.Services.AddSingleton<LoadDataJob>();

        builder.Services.AddSingleton<IMvdWebClient, MvdWebClient>();
        builder.Services.AddSingleton<IMvdFileLoader, MvdFileLoader>();
        builder.Services.AddSingleton<ITemporaryPath, TemporaryPath>();
        builder.Services.AddSingleton<IRepositoryPath, RepositoryPath>();
        builder.Services.AddSingleton<IFileNameRecipient, FileNameRecipient>();

        builder.Services.AddSingleton<IPassportPreparation, PassportPreparation>();
        builder.Services.AddSingleton<IArchiver, Archiver>();
        builder.Services.AddSingleton<IPassportParser, PassportParser>();

        builder.Services.AddSingleton<IFileStorage, Passports.Api.Models.LoadData.Storages.FileStorage>();
        builder.Services.AddScoped<IPassportProvider, Passports.Api.Models.Passport.PassportProviders.FileStorage>();

        builder.Services.AddQuartz(builder.Configuration);
        break;
    case Modes.Redis:
        builder.Services.AddSingleton<IRedisStorage, Passports.Api.Models.LoadData.Storages.RedisStorage>();
        builder.Services.AddScoped<IPassportProvider, Passports.Api.Models.Passport.PassportProviders.RedisStorage>();
        break;
}
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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