using Microsoft.Extensions.Options;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData.Interfaces;

namespace Passports.Api.Models.LoadData;

/// <summary>
/// Парсер данных паспорта
/// </summary>
public class PassportParser : IPassportParser
{
    private readonly Settings _settings;

    public PassportParser(IOptions<Settings> options)
    {
        _settings = options.Value;
    }

    /// <summary>
    /// Парсинг паспортный данных из файла
    /// </summary>
    /// <returns>Список серии и номеров паспортов</returns>
    public Task<Dictionary<uint, List<uint>>> Parse(string fileName)
    {
        if (!FileExists(fileName))
            return Task.FromResult<Dictionary<uint, List<uint>>>(null!);

        var dir = new Dictionary<uint, List<uint>>();
        foreach (var line in FileReadLines(fileName))
        {
            if (string.IsNullOrEmpty(line))
                continue;
            var items = line.Split(',');
            if (items.Length != 2)
                continue;
            if (!TryParse.ElementPassport(items[0], _settings.MaxSeries, out uint s))
                continue;
            if (!TryParse.ElementPassport(items[1], _settings.MaxNumber, out uint n))
                continue;

            if (dir.ContainsKey(s))
                dir[s].Add(n);
            else
                dir[s] = new List<uint> { n };
        }

        return Task.FromResult(dir);
    }

    public virtual bool FileExists(string fileName)
    {
        return File.Exists(fileName);
    }

    public virtual IEnumerable<string> FileReadLines(string fileName)
    {
        return File.ReadLines(fileName);
    }
}