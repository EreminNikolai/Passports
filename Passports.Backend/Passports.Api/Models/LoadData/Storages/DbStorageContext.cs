using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Passports.Api.Exceptions;
using Passports.Api.Models.Dto;
using Passports.Api.Models.LoadData.Interfaces.Storages;

namespace Passports.Api.Models.LoadData.Storages;

/// <summary>
/// Хранилище базы данных
/// </summary>
internal sealed class DbStorageContext : DbContext, IDbStorage
{
    private readonly IMapper _mapper;
    public DbStorageContext(DbContextOptions<DbStorageContext> options, IMapper mapper) : base(options)
    {
        _mapper = mapper;
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

    public async Task<int> Create(Passport.Passport passports)
    {
        await Passports.AddAsync(passports);
        await SaveChangesAsync();
        return 0;
    }

    public async Task Update(Passport.Passport passport)
    {
        var entity = await Passports.FirstOrDefaultAsync(p => p.Id == passport.Id);
        if (entity == null)
            throw new NotFoundException(nameof(Passport), passport.Id);
        entity.Series = passport.Series;
        entity.Number = passport.Number;
        await SaveChangesAsync();
    }

    public async Task<List<PassportDto>> GetAll(int skip, int top)
    {
        return await Passports
            .Skip(skip)
            .Take(top)
            .ProjectTo<PassportDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}