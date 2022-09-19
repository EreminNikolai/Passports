using Microsoft.EntityFrameworkCore;
using Passports.Api.Models.LoadData.Interfaces.Storages;

namespace Passports.Api.Models.LoadData.Storages;

/// <summary>
/// Хранилище базы данных
/// </summary>
internal class DbStorage : DbContext, IDbStorage
{
    public DbStorage(DbContextOptions<DbStorage> options) : base(options)
    {

    }

    /// <summary>
    /// Получить список паспортов
    /// </summary>
    public DbSet<Passport.Passports> Passports { get; set; }

    /// <summary>
    /// Определить наличия паспорта
    /// </summary>
    /// <param name="series">Серия паспорта</param>
    /// <param name="number">Номер паспорта</param>
    /// <returns></returns>
    public async Task<bool> IsPassportExistAsync(uint series, uint number)
    {
        return await Passports.AnyAsync(passports => passports.Series == series && passports.Number == number);
    }
}