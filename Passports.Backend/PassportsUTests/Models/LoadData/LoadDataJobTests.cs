using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Passports.Api.Models.LoadData;
using Passports.Api.Models.LoadData.Interfaces;
using Passports.Api.Models.LoadData.Interfaces.Storages;
using Quartz;

namespace PassportsUTests.Models.LoadData;

[TestFixture]
public class LoadDataJobTests
{
    private MockRepository _repository;
    private Mock<IMvdFileLoader> _mockMvdFileLoader;
    private Mock<IPassportPreparation> _mockPassportPreparation;
    private Mock<IFileStorage> _mockFileStorage;
    private Mock<IJobExecutionContext> _mockJobExecutionContext;
    private KeyValuePair<uint, List<uint>> _pair;
    private Dictionary<uint, List<uint>> _dir;

    [SetUp]
    public void SetUp()
    {
        _pair = new KeyValuePair<uint, List<uint>>(It.IsAny<uint>(), new List<uint> { It.IsAny<uint>() });
        _dir = new Dictionary<uint, List<uint>> { [_pair.Key] = _pair.Value };
        _repository = CreateMockRepository(out _mockMvdFileLoader, out _mockPassportPreparation, out _mockFileStorage, out _mockJobExecutionContext);
    }

    private MockRepository CreateMockRepository(out Mock<IMvdFileLoader> mockMvdFileLoader, out Mock<IPassportPreparation> mockPassportPreparation,
        out Mock<IFileStorage> mockFileStorage, out Mock<IJobExecutionContext> mockJobExecutionContext)
    {

        var repository = new MockRepository(MockBehavior.Strict)
        {
            CallBase = true,
            DefaultValue = DefaultValue.Mock
        };

        //Проверяем последовательность вполнения
        var sequence = new MockSequence();

        //IMvdFileLoader
        mockMvdFileLoader = repository.Create<IMvdFileLoader>();
        mockMvdFileLoader
            .InSequence(sequence)
            .Setup(options => options.DownloadFileUsedMvdWebClientAsync().Result)
            .Returns("zip");

        //IPassportPreparation
        mockPassportPreparation = repository.Create<IPassportPreparation>();
        mockPassportPreparation
            .InSequence(sequence)
            .Setup(options => options.PreparationFromFileAsync(It.IsAny<string>()).Result)
            .Returns(_dir);

        //IFileStorage
        mockFileStorage = repository.Create<IFileStorage>();
        mockFileStorage
            .InSequence(sequence)
            .Setup(storage => storage.Create(_dir))
            .Returns(Task.CompletedTask);

        mockFileStorage
            .Setup(storage => storage.IsPassportExistAsync(_pair.Key, _pair.Value[0]).Result)
            .Returns(true);

        //IJobExecutionContext
        mockJobExecutionContext = repository.Create<IJobExecutionContext>();
        mockJobExecutionContext
            .Setup(context => context.Result)
            .Returns(null);
            
        return repository;
    }

    [Test]
    public async Task ExecuteTest()
    {
        var loadDataJob = new LoadDataJob(_mockMvdFileLoader.Object, _mockPassportPreparation.Object, _mockFileStorage.Object);

        await loadDataJob.Execute(_mockJobExecutionContext.Object).ConfigureAwait(false);

        _repository.Verify();

        _mockMvdFileLoader.Verify(loader => loader.DownloadFileUsedMvdWebClientAsync(), Times.AtLeastOnce);
        _mockMvdFileLoader.VerifyNoOtherCalls();

        _mockPassportPreparation.Verify(preparation => preparation.PreparationFromFileAsync("zip"), Times.AtLeastOnce);
        _mockPassportPreparation.VerifyNoOtherCalls();

        _mockFileStorage.Verify(storage => storage.Create(_dir), Times.AtLeastOnce);
        _mockFileStorage.Verify(storage => storage.IsPassportExistAsync(It.IsAny<uint>(), It.IsAny<uint>()), Times.Never);
        _mockFileStorage.VerifyNoOtherCalls();

        _repository.Verify();
    }
}