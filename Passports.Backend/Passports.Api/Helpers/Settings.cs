namespace Passports.Api.Helpers;

/// <summary>
/// Опции конфиг файла
/// </summary>
public class Settings
{
    /// <summary>
    /// Адрес базы данных
    /// </summary>
    public string Url { get; set; }
    /// <summary>
    /// Дата время старта загрузки данных в формате Cron
    /// </summary>
    public string DownloadCronStartTime { get; set; }
    /// <summary>
    /// Максимально возможная серия
    /// </summary>
    public uint MaxSeries { get; set; }
    /// <summary>
    /// Максимально
    /// возможный номер </summary>
    public uint MaxNumber { get; set; }
    /// <summary>
    /// Имя папки для хранения загруженных данных
    /// </summary>
    public string NameDirectoryData { get; set; }
    /// <summary>
    /// Имя папки файлового хранилища
    /// </summary>
    public string NameDirectoryRepository { get; set; }
}