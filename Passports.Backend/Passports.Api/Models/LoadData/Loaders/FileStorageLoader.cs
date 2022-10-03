using Passports.Api.Models.LoadData.Interfaces;
using Passports.Api.Models.LoadData.Interfaces.Storages;

namespace Passports.Api.Models.LoadData.Loaders;

public class FileStorageLoader: LoaderBase
{
    private readonly IFileStorage _storage;   
    public FileStorageLoader(IMvdFileLoader fileLoader, IFileStorage storage, IPassportPreparation passportPreparation)
        : base(fileLoader, passportPreparation)
    {
        _storage = storage;
    }
    
    protected override async Task StorageCreate(Dictionary<uint, List<uint>> passports)
    {
        await _storage.Create(passports);
    }
}