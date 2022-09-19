using System.IO;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Passports.Api.Models.LoadData.Interfaces;
using Passports.Api.Models.LoadData;

namespace PassportsUTests.Models.LoadData;

[TestFixture]
public class MvdFileLoaderTests
{
    private MockRepository _repository;
    private ITemporaryPath _temporaryPath;
    private IMvdWebClient _mvdWebClient;
    private IFileNameRecipient _fileNameRecipient;
    private string _testFileZip = "C:\\TEST.zip";

    [SetUp]
    public void SetUp()
    {
        _repository = CreateMockRepository(out _temporaryPath, out _mvdWebClient, out _fileNameRecipient);
    }

    private MockRepository CreateMockRepository(out ITemporaryPath temporaryPath, out IMvdWebClient mvdWebClient,
        out IFileNameRecipient fileNameRecipient)
    {
        var repository = new MockRepository(MockBehavior.Strict)
        {
            CallBase = true,
            DefaultValue = DefaultValue.Mock
        };
            
        //ITemporaryPath
        var mockTemporaryPath = repository.Create<ITemporaryPath>();
        mockTemporaryPath.Setup(path => path.GetFullPath(It.IsAny<string>())).Returns(_testFileZip);
        mockTemporaryPath.Setup(path => path.DataFilePath).Returns("Data.csv");
        temporaryPath = mockTemporaryPath.Object;

        //IMvdWebClient
        var mockWebClient = repository.Create<IMvdWebClient>();
        mockWebClient
            .Setup(loader => loader.DownloadFileTaskAsync(_testFileZip))
            .Returns(Task.CompletedTask);
        mvdWebClient = mockWebClient.Object;

        //IFileNameRecipient
        var mockFileNameRecipient = repository.Create<IFileNameRecipient>();
        mockFileNameRecipient
            .Setup(path => path.GetFileNameFromURL().Result)
            .Returns(Path.GetFileName(_testFileZip)); ;
        fileNameRecipient = mockFileNameRecipient.Object;

        return repository;
    }

    [Test]
    public async Task DownloadFileUsedMvdWebClientAsyncTest()
    {
        var fileLoader = new MvdFileLoader(_temporaryPath, _mvdWebClient, _fileNameRecipient);
        var result = await fileLoader.DownloadFileUsedMvdWebClientAsync();
        _repository.Verify();
        // Assert
        Assert.AreEqual(_testFileZip, result);
    }

    [Test]
    public async Task WebClientDownloadFileTaskAsyncTest()
    {
        var mockerFileLoader = _repository.Create<MvdFileLoader>(MockBehavior.Default, _temporaryPath, _mvdWebClient, _fileNameRecipient);
        mockerFileLoader
            .Setup(e => e.WebClientDownloadFileTaskAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var result = await mockerFileLoader.Object.DownloadFileUsedWebClientAsync().ConfigureAwait(false);
        mockerFileLoader.Verify(e => e.WebClientDownloadFileTaskAsync(It.IsAny<string>()), Times.AtLeastOnce);
        _repository.Verify();

        // Assert
        Assert.AreEqual(_testFileZip, result, "Результат метода не соответвует тестовому результату");
            
    }
}