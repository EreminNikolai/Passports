using System.Net;
using Microsoft.Extensions.Options;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData.Interfaces;

namespace Passports.Api.Models.LoadData;

/// <summary>
/// Загрузка файла МВД
/// </summary>
internal class MvdWebClient : IMvdWebClient
{
    private readonly Settings _settings;
    public MvdWebClient(IOptions<Settings> settings)
    {
        _settings = settings.Value;
    }
    /// <summary>
    /// Загрузить файл с указанным именем 
    /// </summary>
    /// <param name="fileName">Имя файла</param>
    /// <returns></returns>
    public async Task DownloadFileTaskAsync(string fileName)
    {
        using var client = new WebClient();
        await client.DownloadFileTaskAsync(_settings.Url, fileName).ConfigureAwait(false);
    }

    /// <summary>
    /// Адрес по которому производится загрузка файла
    /// </summary>
    public string Url => _settings.Url;
}