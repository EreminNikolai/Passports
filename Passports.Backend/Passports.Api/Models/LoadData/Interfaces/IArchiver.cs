namespace Passports.Api.Models.LoadData.Interfaces;

/// <summary>
/// Архиватор
/// </summary>
public interface IArchiver
{
    /// <summary> Разархивировать </summary>
    /// <param name="fileArch">Имя файла архива</param>
    /// <param name="isRemoveSourceFile">Необходимость удаления архива после разархивации</param>
    /// <returns></returns>
    Task<string> Decompress(string fileArch, bool isRemoveSourceFile);
}