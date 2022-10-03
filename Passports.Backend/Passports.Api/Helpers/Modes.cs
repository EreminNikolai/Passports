namespace Passports.Api.Helpers;

/// <summary>
/// Режимы работы программы
/// </summary>
internal enum Modes
{
    /// <summary> База данных Postgres</summary>
    Postgres = 0,
    /// <summary> Файловая система </summary>
    FileStorage = 1,
    /// <summary> Сервис Redis </summary>
    Redis = 2
}