namespace Passports.Api.Helpers;

/// <summary>
/// Парсер
/// </summary>
internal static class TryParse
{
    /// <summary>
    /// Парсер элемента паспорта
    /// </summary>
    /// <param name="param">Входящее значение</param>
    /// <param name="maxValue">Максмальное значение</param>
    /// <param name="result">Результат выполнение операции</param>
    /// <returns>Состояние выполеннеия операция </returns>
    public static bool ElementPassport(string param, uint maxValue, out uint result) =>
        uint.TryParse(param, out result) && result >= 1 && result <= maxValue;
}