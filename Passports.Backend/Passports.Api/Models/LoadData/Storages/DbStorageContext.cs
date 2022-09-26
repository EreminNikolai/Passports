using Microsoft.EntityFrameworkCore;
using Passports.Api.Models.LoadData.Interfaces.Storages;

namespace Passports.Api.Models.LoadData.Storages;

/// <summary>
/// Хранилище базы данных
/// </summary>
internal sealed class DbStorageContext : DbContext, IDbStorage
{
    public DbStorageContext(DbContextOptions<DbStorageContext> options) : base(options)
    {
        
    }

    /// <summary>
    /// Получить список паспортов
    /// </summary>
    public DbSet<Passport.Passport> Passports { get; set; }

    public async Task Create(Dictionary<uint, List<uint>> series)
    {
        var passports = (
            from seriesItem in series 
            from number in seriesItem.Value 
            select new Passport.Passport { Series = seriesItem.Key, Number = number })
            .ToList();
        await Passports.AddRangeAsync(passports);
        await SaveChangesAsync();
    }

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