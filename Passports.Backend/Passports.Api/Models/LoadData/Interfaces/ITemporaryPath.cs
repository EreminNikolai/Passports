namespace Passports.Api.Models.LoadData.Interfaces;

/// <summary>
/// Папка с временными файлами
/// </summary>
public interface ITemporaryPath
{
    /// <summary>
    /// Путь к файлу csv
    /// </summary>
    string DataFilePath { get; }

    /// <summary>
    /// Получить полный путь
    /// </summary>
    /// </summary>
    /// <param name="elementPath">Наименование папки или файла для получения пути</param>
    /// <returns></returns>
    string GetFullPath(string elementPath);

    /// <summary>
    /// Удалить файл Data.csv
    /// </summary>
    void DeleteDataFile();
}