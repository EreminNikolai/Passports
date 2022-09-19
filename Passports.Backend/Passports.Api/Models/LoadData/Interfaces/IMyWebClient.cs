namespace Passports.Api.Models.LoadData.Interfaces;

/// <summary>
/// Загрузка файла МВД
/// </summary>
public interface IMvdWebClient
{
    /// <summary>
    /// Загрузить файл с указанным именем 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    Task DownloadFileTaskAsync(string fileName);

    /// <summary>
    /// Адрес по которому производится загрузка файла
    /// </summary>
    string Url { get; }
}