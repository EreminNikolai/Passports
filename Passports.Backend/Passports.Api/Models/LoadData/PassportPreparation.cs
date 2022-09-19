using Passports.Api.Models.LoadData.Interfaces;

namespace Passports.Api.Models.LoadData;

/// <summary>
/// Подготовка данных
/// </summary>
public class PassportPreparation: IPassportPreparation
{
    private readonly IArchiver _archiver;
    private readonly IPassportParser _passportParser;
    private readonly ITemporaryPath _temporaryPath;

    public PassportPreparation(IArchiver archiver, IPassportParser passportParser, ITemporaryPath temporaryPath)
    {
        _archiver = archiver;
        _passportParser = passportParser;
        _temporaryPath = temporaryPath;
    }
    /// <summary>
    /// Подготовить данные из файла
    /// </summary>
    /// <param name="fileName">Путь к файлу</param>
    /// <returns></returns>
    public async Task<Dictionary<uint, List<uint>>> PreparationFromFileAsync(string fileName)
    {
        var fileResult = await _archiver.Decompress(fileName, true).ConfigureAwait(false);
        var result = await _passportParser.Parse(fileResult)
            .ContinueWith(t=>
            {
                _temporaryPath.DeleteDataFile();
                return t;
            })
            .ConfigureAwait(false);
        return await result;
    }
}