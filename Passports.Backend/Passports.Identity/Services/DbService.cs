using Microsoft.EntityFrameworkCore;
using Passports.Identity.Data;

namespace Passports.Api.Services;

/// <summary>
/// Расширение для подключения базы данных
/// </summary>
public static class DbService
{
    /// <summary>
    /// Добавление сервиса Postgres
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="environment"></param>
    /// <param name="logger"></param>
    public static void AddPostgres(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetValue<string>("DbConnection");
        builder.Services.AddDbContext<AuthDbContext>(opt => opt.UseNpgsql(connectionString));
    }

    /// <summary>
    /// Добавление сервиса EF
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="environment"></param>
    /// <param name="logger"></param>
    public static void AddSqlite(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetValue<string>("DbConnection");
        builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlite(connectionString));
    }


    public static void UseDb(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        //using var context = serviceScope.ServiceProvider.GetService<AuthDbContext>();
        using var context = serviceScope.ServiceProvider.GetRequiredService<AuthDbContext>();
        context?.Database.EnsureCreated();

        //using var scope = app.ApplicationServices.CreateScope();
        //var serviceProvider = scope.ServiceProvider;
        //try
        //{
        //    var context = serviceProvider.GetRequiredService<AuthDbContext>();
        //    context.Database.EnsureCreated();
        //}
        //catch (Exception exception)
        //{
        //    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        //    logger.LogError(exception, "An error occurred while app initialization");
        //}
    }
}