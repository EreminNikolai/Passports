using Passports.Api.Models.LoadData.Interfaces;
using Passports.Api.Models.LoadData.Interfaces.Storages;
using Passports.Api.Models.LoadData.Loaders;
using Quartz;

namespace Passports.Api.Models.LoadData;

/// <summary>
/// Загрузка данных и формирование файлового хранилища
/// </summary>
//[DisallowConcurrentExecution]
public class LoadDataJob: IJob 
{
    private readonly ILoader _loader;

    public LoadDataJob(ILoader loader)
    {
        _loader = loader;
    }

    /// <summary>
    /// Запустить задачу
    /// </summary>
    /// <param name="context">Контекст</param>
    /// <returns></returns>
    public async Task Execute(IJobExecutionContext context)
    {
        await _loader.LoadAsync();
    }
}