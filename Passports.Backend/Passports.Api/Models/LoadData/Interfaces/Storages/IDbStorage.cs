using Passports.Api.Models.Dto;

namespace Passports.Api.Models.LoadData.Interfaces.Storages;

/// <summary>
/// Хранилище в базе данных
/// </summary>
public interface IDbStorage : IStorageBase
{
    /// <summary>
    /// Создать паспорт
    /// </summary>
    /// <param name="passports">Данные, которые необходимо сохранить в хранилище</param>
    /// <returns></returns>
    Task<int> Create(Passport.Passport passports);

    /// <summary>
    /// Обновить паспорт
    /// </summary>
    /// <param name="passports">Данные, которые необходимо сохранить в хранилище</param>
    /// <returns></returns>
    Task Update(Passport.Passport passports);

    /// <summary>
    /// Получить список паспортов
    /// </summary>
    /// <returns></returns>
    Task<List<PassportDto>> GetAll(int skip, int top);
}