using Passports.Api.Models.LoadData.Interfaces;
using Passports.Api.Models.LoadData.Interfaces.Storages;

namespace Passports.Api.Models.LoadData.Loaders;

public class LoaderBase: ILoader 
{
    private readonly IMvdFileLoader _fileLoader;
    private readonly IPassportPreparation _passportPreparation;

    protected LoaderBase(IMvdFileLoader fileLoader, IPassportPreparation passportPreparation)
    {
        _fileLoader = fileLoader;
        _passportPreparation = passportPreparation;
    }

    protected virtual async Task StorageCreate(Dictionary<uint, List<uint>> passports)
    {
        await Task.CompletedTask;
    }

    public async Task LoadAsync()
    {
        var fileZip = await _fileLoader.DownloadFileUsedMvdWebClientAsync();
        var result = await _passportPreparation.PreparationFromFileAsync(fileZip);
        await StorageCreate(result);
    }
}