using Microsoft.Extensions.Options;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData.Interfaces;

namespace Passports.Api.Models.LoadData;

/// <summary>
/// Папка с временными файлами
/// </summary>
public class TemporaryPath : ITemporaryPath
{
    private string _dataPath;
    private readonly Settings _setting;

    public TemporaryPath(IOptions<Settings> options)
    {
        _setting = options.Value;
    }

    public string DataPath
    {
        get
        {
            if (!string.IsNullOrEmpty(_dataPath))
                return _dataPath;
            _dataPath = Path.Combine(Environment.CurrentDirectory, _setting.NameDirectoryData);
            CreateDirectory();
            return _dataPath;
        }
    }

    /// <summary>
    /// Получить полный путь
    /// </summary>
    /// <param name="elementPath">Наименование папки или файла для получения пути</param>
    /// <returns></returns>
    public string GetFullPath(string elementPath) => Path.Combine(DataPath, elementPath);
        
    /// <summary>
    /// Путь к файлу csv
    /// </summary>
    public string DataFilePath => GetFullPath("Data.csv");

    /// <summary>
    /// Удалить файл Data.csv
    /// </summary>
    public void DeleteDataFile()
    {
        if(File.Exists(DataFilePath))
            File.Delete(DataFilePath);
    }

    public virtual void CreateDirectory()
    {
        if (!Directory.Exists(_dataPath))
            Directory.CreateDirectory(_dataPath);
    }
}