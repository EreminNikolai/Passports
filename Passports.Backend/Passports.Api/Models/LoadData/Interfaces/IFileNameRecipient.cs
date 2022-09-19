namespace Passports.Api.Models.LoadData.Interfaces;

/// <summary>
/// Получатель имени файла
/// </summary>
public interface IFileNameRecipient
{
    /// <summary>
    /// Получить имя файла из URL
    /// </summary>
    /// <returns></returns>
    Task<string> GetFileNameFromURL();
}