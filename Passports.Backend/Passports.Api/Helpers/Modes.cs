namespace Passports.Api.Helpers;

/// <summary>
/// Режимы работы программы
/// </summary>
internal enum Modes
{
    /// <summary> База данных </summary>
    Database = 0,
    /// <summary> Файловая система </summary>
    FileStorage = 1,
    /// <summary> Сервис Redis </summary>
    Redis = 2
}