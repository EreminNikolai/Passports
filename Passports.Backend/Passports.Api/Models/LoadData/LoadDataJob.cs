using Passports.Api.Models.LoadData.Interfaces;
using Passports.Api.Models.LoadData.Interfaces.Storages;
using Quartz;

namespace Passports.Api.Models.LoadData;

/// <summary>
/// Загрузка данных и формирование файлового хранилища
/// </summary>
[DisallowConcurrentExecution]
public class LoadDataJob : IJob
{
    private readonly IMvdFileLoader _imvdFileLoader;
    private readonly IFileStorage _fileStorage;
    private readonly IPassportPreparation _passportPreparation;

    public LoadDataJob(IMvdFileLoader imvdFileLoader, IPassportPreparation passportPreparation, IFileStorage fileStorage)
    {
        _imvdFileLoader = imvdFileLoader;
        _passportPreparation = passportPreparation;
        _fileStorage = fileStorage;
    }

    /// <summary>
    /// Запустить задачу
    /// </summary>
    /// <param name="context">Контекст</param>
    /// <returns></returns>
    public async Task Execute(IJobExecutionContext context)
    {
        string fileZip = await _imvdFileLoader.DownloadFileUsedMvdWebClientAsync().ConfigureAwait(false);
        var result = await _passportPreparation.PreparationFromFileAsync(fileZip).ConfigureAwait(false);
        await _fileStorage.Create(result).ConfigureAwait(false);
    }
}