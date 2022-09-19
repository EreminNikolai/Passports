using Passports.Api.Models.LoadData;
using Quartz;

namespace Passports.Api.Services;

/// <summary>
/// Класс расшерение для Quartz
/// </summary>
public static class QuartzService
{
    /// <summary>
    /// Добавление и настройка сервиса Quartz
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddQuartz(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(q =>
        {
            q.SchedulerId = "Scheduler-Core";
            q.UseMicrosoftDependencyInjectionJobFactory(options => { options.AllowDefaultConstructor = true; });
            q.UseSimpleTypeLoader();
            q.UseInMemoryStore();
            q.UseDefaultThreadPool(tp => { tp.MaxConcurrency = 5; });

            var jobKey = new JobKey("DataLoadTriggerKey", "DataLoadTriggerGroup");
            q.AddJob<LoadDataJob>(jobKey, j => j
                .WithDescription("Start loading data")
            );

            q.AddTrigger(t => t
                .WithIdentity("DataLoadCronTriggerKey")
                .ForJob(jobKey)
                .WithCronSchedule(configuration["Settings:DownloadCronStartTime"])
                .WithDescription("Start loading data")
                .StartNow()
            );
        });

        services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });
    }
}