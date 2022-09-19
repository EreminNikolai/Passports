using Microsoft.Extensions.Options;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData.Interfaces;
using Passports.Api.Models.LoadData.Interfaces.Storages;

namespace Passports.Api.Models.LoadData.Storages;

/// <summary>
/// Файловое хранилище
/// </summary>
public class FileStorage : IFileStorage
{
    private readonly Settings _settings;
    private readonly IRepositoryPath _repositoryPath;

    public FileStorage(IRepositoryPath repositoryPath, IOptions<Settings> options)
    {
        _settings = options.Value;
        _repositoryPath = repositoryPath;
    }

    /// <summary>
    /// Создать файловое хранилище
    /// </summary>
    /// <param name="folderRepository">Путь к папке</param>
    /// <param name="dir">Данные, которые необходимо сохранить в папку</param>
    /// <returns></returns>
    public Task Create(Dictionary<uint, List<uint>> dir)
    {
        try
        {
            Parallel.ForEach(dir.Keys, new ParallelOptions { MaxDegreeOfParallelism = 10 },
                key => CreateFile(key.ToString(), dir[key]).ConfigureAwait(false));
        }
        catch (Exception ex)
        {
            return Task.FromException(ex);
        }

        return Task.CompletedTask;
    }

    public virtual Task CreateFile(string series, List<uint> numbers)
    {
        numbers ??= new List<uint>();

        var stack = new Stack<uint>(numbers.OrderByDescending(t => t).ToList());

        series = series.PadLeft(4, '0');

        var fileName = _repositoryPath.GetDirectoryRepositoryPath($"{series}.pst");

        uint? firstNumber = stack.Pop();

        if (File.Exists(fileName))
            File.Delete(fileName);
        using var writer = new BinaryWriter(File.Open(fileName, FileMode.OpenOrCreate));
        for (uint i = 0; i < _settings.MaxNumber; i++)
        {
            if (i == firstNumber)
            {
                writer.Write(true);
                firstNumber = stack.Count == 0
                    ? (uint?)null
                    : stack.Pop();
            }
            else
                writer.Write(false);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Определить наличия паспорта
    /// </summary>
    /// <param name="series">Серия паспорта</param>
    /// <param name="number">Номер паспорта</param>
    /// <returns></returns>
    public async Task<bool> IsPassportExistAsync(uint series, uint number)
    {
        return await Task.Run(() =>
        {
            var fileName = _repositoryPath.GetDirectoryRepositoryPath($"{series}.pst");
            try
            {
                return ReadNumberFile(fileName, number);
            }
            catch
            {
                return false;
            }
        }).ConfigureAwait(false);
    }

    public virtual bool ReadNumberFile(string fileName, uint number)
    {
        if (!File.Exists(fileName))
            return false;
        uint index = 0;
        using BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open));
        while (reader.PeekChar() > -1)
        {
            var value = reader.ReadBoolean();
            if (number == index)
                return value;
            index++;
        }

        return false;
    }
}