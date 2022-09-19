using Microsoft.Extensions.Options;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData.Interfaces;

namespace Passports.Api.Models.LoadData;

/// <summary>
/// Получатель имени файла
/// </summary>
public class FileNameRecipient: IFileNameRecipient
{
    private readonly Settings _settings;
    public FileNameRecipient (IOptions<Settings> settings)
    {
        _settings = settings.Value;
    }

    /// <summary>
    /// Получить имя файла из URL
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetFileNameFromURL()
    {
        string fileName = PathGetFileName(_settings.Url);
        if (string.IsNullOrEmpty(fileName))
            throw new Exception($"Из строки Url {_settings.Url} не удалось выделить имя файла");
        return await Task.FromResult(fileName);
    }

    /// <summary>
    /// Возвращает имя и расширение заданного пути.
    /// </summary>
    /// <param name="path">Путь к файлу</param>
    /// <returns></returns>
    public virtual string PathGetFileName(string path)
    {
        return Path.GetFileName(_settings.Url);
    }
}