using Passports.Api.Models.LoadData.Interfaces;
using Passports.Api.Models.LoadData.Interfaces.Storages;

namespace Passports.Api.Models.LoadData.Loaders;

public class RedisLoader: LoaderBase
{
    private readonly IRedisStorage _storage;
    public RedisLoader(IMvdFileLoader fileLoader, IRedisStorage storage, IPassportPreparation passportPreparation)
        : base(fileLoader, passportPreparation)
    {
        _storage = storage;
    }

    protected override async Task StorageCreate(Dictionary<uint, List<uint>> passports)
    {
        await _storage.Create(passports);
    }
}