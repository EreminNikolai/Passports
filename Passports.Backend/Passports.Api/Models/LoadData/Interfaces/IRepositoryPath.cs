namespace Passports.Api.Models.LoadData.Interfaces;

/// <summary>
/// Путь к файловому хранилищу
/// </summary>
public interface IRepositoryPath
{
    /// <summary>
    /// Полный путь к папке с файловым хранилищем
    /// </summary>
    string DirectoryRepositoryPath { get; }

    /// <summary>
    /// Получить полный путь к указанной папке или файлу для папки файлового хранилища
    /// </summary>
    /// <param name="elementPath">Наименование папки или файла для получения пути</param>
    /// <returns></returns>
    string GetDirectoryRepositoryPath(string elementPath);
}