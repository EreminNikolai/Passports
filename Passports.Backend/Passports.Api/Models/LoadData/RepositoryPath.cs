using Microsoft.Extensions.Options;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData.Interfaces;

namespace Passports.Api.Models.LoadData;

/// <summary>
/// Путь к файловому хранилищу
/// </summary>
public class RepositoryPath: IRepositoryPath
{
    private string _directoryRepositoryPath;
    private readonly Settings _settings;

    public RepositoryPath(IOptions<Settings> options)
    {
        _settings = options.Value;
    }

    /// <summary>
    /// Полный путь к папке с файловым хранилищем
    /// </summary>
    public string DirectoryRepositoryPath
    {
        get
        {
            if (!string.IsNullOrEmpty(_directoryRepositoryPath)) 
                return _directoryRepositoryPath;

            _directoryRepositoryPath = Path.Combine(Environment.CurrentDirectory, _settings.NameDirectoryRepository);
            CreateDirectory();
            return _directoryRepositoryPath;
        }
    }

    /// <summary>
    /// Получить полный путь к указанной папке или файлу для папки файлового хранилища
    /// </summary>
    /// <param name="elementPath">Наименование папки или файла для получения пути</param>
    /// <returns></returns>
    public string GetDirectoryRepositoryPath(string elementPath) => Path.Combine(DirectoryRepositoryPath, elementPath);

    public virtual void CreateDirectory()
    {
        if (!Directory.Exists(_directoryRepositoryPath))
            Directory.CreateDirectory(_directoryRepositoryPath);
    }
}