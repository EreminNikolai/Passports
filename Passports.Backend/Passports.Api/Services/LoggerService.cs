namespace Passports.Api.Services;

public static class LoggerService
{
    public static void AddLogger(this IServiceCollection services, IWebHostEnvironment environment)
    {
        var dirLog = environment.IsDevelopment()
            ? Path.Combine(Directory.GetCurrentDirectory(), "logs")
            : "/home/logs";

        NLog.GlobalDiagnosticsContext.Set("appbasepath", dirLog);
    }
}
