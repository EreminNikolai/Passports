using Microsoft.Extensions.Options;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData.Interfaces.Storages;
using Passports.Api.Models.Passport.Interfaces;

namespace Passports.Api.Models.Passport.PassportProviders;

/// <summary>
/// Провайдер работы с паспортом через Redis
/// </summary>
public class RedisStorage: IPassportProvider
{
    private readonly IRedisStorage _redisStorage;
    private readonly ILogger<RedisStorage> _logger;
    private readonly Settings _settings;

    public RedisStorage(IRedisStorage redisStorage, IOptions<Settings> options, ILogger<RedisStorage> logger)
    {
        _redisStorage = redisStorage;
        _settings = options.Value;
        _logger = logger;
        _logger.LogDebug("NLog injected into PassportProvider.DataBase");
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

        var result = await _redisStorage.IsPassportExistAsync(seriesUInt, numberUInt);
        _logger.LogInformation($"Exists({series}, {number})={result}");
        return result;
    }
}