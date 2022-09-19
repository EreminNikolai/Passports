namespace Passports.Api.Models.LoadData.Interfaces;

/// <summary>
/// Загрузка файла
/// </summary>
public interface IMvdFileLoader
{
    /// <summary>
    /// Загрузить файл МВД через интрефейс IMvdWebClient
    /// </summary>
    /// <returns></returns>
    Task<string> DownloadFileUsedMvdWebClientAsync();

    /// <summary>
    /// Загрузить файл МВД через метод WebClientDownloadFileTaskAsync
    /// </summary>
    /// <returns></returns>
    Task<string> DownloadFileUsedWebClientAsync();
}