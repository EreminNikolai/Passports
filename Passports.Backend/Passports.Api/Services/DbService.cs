using Microsoft.EntityFrameworkCore;
using Passports.Api.Models.LoadData.Storages;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Passports.Api.Services;

/// <summary>
/// Расширение для подключения базы данных
/// </summary>
public static class DbService
{
    /// <summary>
    /// Добавление сервиса EF
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="environment"></param>
    /// <param name="logger"></param>
    public static void AddPostgres(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment, ILogger logger)
    {
        var sqlConnectionString = configuration.GetConnectionString("DataAccessPostgreSqlProvider");
        logger?.LogInformation($"Debug = {environment.IsDevelopment()}. SqlConnectionString = {sqlConnectionString}");
        services.AddDbContext<DbStorage>(options => options.UseNpgsql(sqlConnectionString));
    }

    public static void UsePostgres(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<DbStorage>();
        context?.Database.Migrate();
    }
}