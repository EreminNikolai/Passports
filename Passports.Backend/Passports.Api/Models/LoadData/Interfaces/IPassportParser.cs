namespace Passports.Api.Models.LoadData.Interfaces;

/// <summary>
/// Парсер данных паспорта
/// </summary>
public interface IPassportParser
{
    /// <summary>
    /// Парсинг паспортный данных из файла
    /// </summary>
    /// <returns>Список серии и номеров паспортов</returns>
    Task<Dictionary<uint, List<uint>>> Parse(string fileName);
}