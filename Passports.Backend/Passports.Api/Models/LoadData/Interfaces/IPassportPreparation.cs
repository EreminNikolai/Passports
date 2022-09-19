namespace Passports.Api.Models.LoadData.Interfaces;

/// <summary>
/// Инструмент для подготовки данных
/// </summary>
public interface IPassportPreparation
{
    /// <summary>
    /// Подготовить данных из файла
    /// </summary>
    /// <param name="fileName">Путь к файлу</param>
    /// <returns></returns>
    Task<Dictionary<uint, List<uint>>> PreparationFromFileAsync(string fileName);
}