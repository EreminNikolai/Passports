namespace Passports.Api.Models.LoadData.Interfaces.Storages;

/// <summary>
/// Хранилище в Redis
/// </summary>
public interface IRedisStorage
{
    /// <summary>
    /// Определить наличия паспорта
    /// </summary>
    /// <param name="series">Серия паспорта</param>
    /// <param name="number">Номер паспорта</param>
    /// <returns></returns>
    Task<bool> IsPassportExistAsync(uint series, uint number);
}