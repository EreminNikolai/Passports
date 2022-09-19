using System.Net;
using Passports.Api.Models.LoadData.Interfaces;

namespace Passports.Api.Models.LoadData;

/// <summary>
/// Загрузка файла МВД с сайта
/// </summary>
public class MvdFileLoader: IMvdFileLoader
{
    private readonly ITemporaryPath _temporaryPath;
    private readonly IMvdWebClient _mvdWebClient;
    private readonly IFileNameRecipient _fileNameRecipient;

    public MvdFileLoader(ITemporaryPath temporaryPath, IMvdWebClient mvdWebClient, IFileNameRecipient fileNameRecipient)
    {
        _temporaryPath = temporaryPath;
        _mvdWebClient = mvdWebClient;
        _fileNameRecipient = fileNameRecipient;
    }
    /// <summary>
    /// Загрузить файл МВД через интрефейс IMvdWebClient
    /// </summary>
    /// <returns></returns>
    public async Task<string> DownloadFileUsedMvdWebClientAsync()
    {
        string fileName = await _fileNameRecipient.GetFileNameFromURL().ConfigureAwait(false);
        var fileZip = _temporaryPath.GetFullPath(fileName);
        await _mvdWebClient.DownloadFileTaskAsync(fileZip).ConfigureAwait(false);
        return await Task.FromResult(fileZip);
    }
    /// <summary>
    /// Загрузить файл МВД через метод WebClientDownloadFileTaskAsync
    /// </summary>
    /// <returns></returns>
    public async Task<string> DownloadFileUsedWebClientAsync()
    {
        string fileName = await _fileNameRecipient.GetFileNameFromURL().ConfigureAwait(false);
        var fileZip = _temporaryPath.GetFullPath(fileName);
        await WebClientDownloadFileTaskAsync(fileZip).ConfigureAwait(false);
        return await Task.FromResult(fileZip);
    }

    /// <summary>
    /// Реализация WebClient.WebClientDownloadFileTaskAsync
    /// </summary>
    /// <param name="fileZip"></param>
    /// <returns></returns>
    public virtual async Task WebClientDownloadFileTaskAsync(string fileZip)
    {
        using var client = new WebClient();
        await client.DownloadFileTaskAsync(_mvdWebClient.Url, fileZip).ConfigureAwait(false);
    }
}