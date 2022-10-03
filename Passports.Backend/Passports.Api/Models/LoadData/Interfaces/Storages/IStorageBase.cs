namespace Passports.Api.Models.LoadData.Interfaces.Storages;

public interface IStorageBase
{
    /// <summary>
    /// Создать хранилище
    /// </summary>
    /// <param name="passports">Данные, которые необходимо сохранить в хранилище</param>
    /// <returns></returns>
    Task Create(Dictionary<uint, List<uint>> passports);

    /// <summary>
    /// Определить наличия паспорта
    /// </summary>
    /// <param name="series">Серия паспорта</param>
    /// <param name="number">Номер паспорта</param>
    /// <returns></returns>
    Task<bool> IsPassportExistAsync(uint series, uint number);
}