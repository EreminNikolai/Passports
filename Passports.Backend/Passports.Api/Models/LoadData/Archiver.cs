using ICSharpCode.SharpZipLib.BZip2;
using Passports.Api.Models.LoadData.Interfaces;

namespace Passports.Api.Models.LoadData;

/// <summary>
/// Архиватор
/// </summary>
internal class Archiver: IArchiver
{
    private readonly ITemporaryPath _temporaryPath;
    public Archiver(ITemporaryPath temporaryPath)
    {
        _temporaryPath = temporaryPath;
    }
    /// <summary> Разархивировать </summary>
    /// <param name="fileArch">Имя файла архива</param>
    /// <param name="isRemoveSourceFile">Необходимость удаления архива после разархивации</param>
    /// <returns></returns>
    public Task<string> Decompress(string fileArch, bool isRemoveSourceFile)
    {
        var sourceFileName = _temporaryPath.GetFullPath(fileArch);
        var destinationFileName = _temporaryPath.DataFilePath;
        BZip2.Decompress(File.OpenRead(sourceFileName), File.Create(destinationFileName), true);
        if (isRemoveSourceFile)
            File.Delete(sourceFileName);

        return Task.FromResult(destinationFileName);
    }
}