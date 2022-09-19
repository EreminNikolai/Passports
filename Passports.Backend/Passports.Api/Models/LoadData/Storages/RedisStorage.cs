using Passports.Api.Models.LoadData.Interfaces.Storages;
using StackExchange.Redis;

namespace Passports.Api.Models.LoadData.Storages;

/// <summary>
/// Хранилище Redis
/// </summary>
internal class RedisStorage: IRedisStorage
{
    private readonly ConnectionMultiplexer _connection;
    private readonly ILogger<RedisStorage> _logger;

    public RedisStorage(IConfiguration config, ILogger<RedisStorage> logger)
    {
        _logger = logger;
        try
        {
            var connectionString = config["Redis:ConnectionStrings"];
            _connection = ConnectionMultiplexer.Connect(connectionString);
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogError("Ошибка подключения к Redis. " + ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError("Ошибка Redis. " + ex.Message);
        }
    }

    /// <summary>
    /// Определить наличия паспорта
    /// </summary>
    /// <param name="series">Серия паспорта</param>
    /// <param name="number">Номер паспорта</param>
    /// <returns></returns>
    public async Task<bool> IsPassportExistAsync(uint series, uint number)
    {
        if (_connection == null)
            return await Task.FromResult(false);

        var result = await _connection
            .GetDatabase()
            .SetContainsAsync(new RedisKey(series.ToString()), new RedisValue(number.ToString()))
            .ConfigureAwait(false);
        _logger.LogInformation($"Exists({series}, {number})={result}");
        return result;
    }
}