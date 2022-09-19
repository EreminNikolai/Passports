namespace Passports.Api.Models.LoadData.Interfaces.Storages;

/// <summary>
/// Хранилище в базе данных
/// </summary>
public interface IDbStorage
{
    /// <summary>
    /// Определить наличия паспорта
    /// </summary>
    /// <param name="series">Серия паспорта</param>
    /// <param name="number">Номер паспорта</param>
    /// <returns></returns>
    Task<bool> IsPassportExistAsync(uint series, uint number);
}