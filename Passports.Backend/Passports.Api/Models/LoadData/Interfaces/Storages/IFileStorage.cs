namespace Passports.Api.Models.LoadData.Interfaces.Storages;

/// <summary>
/// Файловое хранилище
/// </summary>
public interface IFileStorage
{
    /// <summary>
    /// Создать файловое хранилище
    /// </summary>
    /// <param name="folderRepository">Путь к папке</param>
    /// <param name="dir">Данные, которые необходимо сохранить в папку</param>
    /// <returns></returns>
    Task Create(Dictionary<uint, List<uint>> dir);

    /// <summary>
    /// Определить наличия паспорта
    /// </summary>
    /// <param name="series">Серия паспорта</param>
    /// <param name="number">Номер паспорта</param>
    /// <returns></returns>
    Task<bool> IsPassportExistAsync(uint series, uint number);
}