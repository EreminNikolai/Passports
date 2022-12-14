using Microsoft.Extensions.Options;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData.Interfaces.Storages;
using Passports.Api.Models.Passport.Interfaces;

namespace Passports.Api.Models.Passport.PassportProviders;

/// <summary>
/// Провайдер работы с паспортом через файловое хранилище
/// </summary>
public class FileStorage : IPassportProvider
{
    private readonly IFileStorage _fileStorage;
    private readonly Settings _settings;
    private readonly ILogger<FileStorage> _logger;

    public FileStorage(IFileStorage fileStorage, IOptions<Settings> options, ILogger<FileStorage> logger)
    {
        _fileStorage = fileStorage;
        _settings = options.Value;
        _logger = logger;
        _logger.LogDebug("NLog injected into PassportProvider.FileStorage");
    }

    /// <summary> Проверка на наличие паспорта </summary>
    /// <param name="series">Серия паспорта</param>
    /// <param name="number">Номер паспорта</param>
    /// <returns></returns>
    public async Task<bool> Exists(string series, string number)
    {
        if (!TryParse.ElementPassport(series, _settings.MaxSeries, out var seriesUInt))
            return false;

        if (!TryParse.ElementPassport(number, _settings.MaxNumber, out var numberUInt))
            return false;

        var result = await _fileStorage.IsPassportExistAsync(seriesUInt, numberUInt).ConfigureAwait(false);
        _logger.LogInformation($"Exists({series}, {number})={result}");
        return result;
    }
}