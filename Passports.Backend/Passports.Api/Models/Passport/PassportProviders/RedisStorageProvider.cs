using Microsoft.Extensions.Options;
using Passports.Api.Helpers;
using Passports.Api.Models.Dto;
using Passports.Api.Models.LoadData.Interfaces.Storages;
using Passports.Api.Models.Passport.Interfaces;

namespace Passports.Api.Models.Passport.PassportProviders;

/// <summary>
/// Провайдер работы с паспортом через Redis
/// </summary>
public class RedisStorageProvider: IPassportProvider
{
    private readonly IRedisStorage _redisStorage;
    private readonly ILogger<RedisStorageProvider> _logger;
    private readonly Settings _settings;

    public RedisStorageProvider(IRedisStorage redisStorage, IOptions<Settings> options, ILogger<RedisStorageProvider> logger)
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

    public async Task<int> Create(Passport passport)
    {
        return await Task.FromResult(0);
    }

    public async Task Update(Passport passport)
    {
        await Task.CompletedTask;
    }

    public async Task<List<PassportDto>> GetAll(int skip, int top)
    {
        return await Task.FromResult(new List<PassportDto>());
    }
}