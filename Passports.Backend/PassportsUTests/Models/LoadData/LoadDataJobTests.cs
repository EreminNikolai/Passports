using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Passports.Api.Models.LoadData;
using Passports.Api.Models.LoadData.Interfaces;
using Quartz;

namespace PassportsUTests.Models.LoadData;

[TestFixture]
public class LoadDataJobTests
{
    private MockRepository _repository;
    private Mock<ILoader> _mockLoader;
    private Mock<IJobExecutionContext> _mockJobExecutionContext;

    [SetUp]
    public void SetUp()
    {
        _repository = CreateMockRepository(
            out _mockLoader,
            out _mockJobExecutionContext);
    }

    private MockRepository CreateMockRepository(
        out Mock<ILoader> mockLoader,
        out Mock<IJobExecutionContext> mockJobExecutionContext)
    {

        var repository = new MockRepository(MockBehavior.Default)
        {
            CallBase = true,
            DefaultValue = DefaultValue.Mock
        };

        //Проверяем последовательность вполнения
        var sequence = new MockSequence();
        
        //ILoader
        mockLoader = repository.Create<ILoader>();
        mockLoader
            .InSequence(sequence)
            .Setup(options => options.LoadAsync());
        
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
        var loadDataJob = new LoadDataJob(_mockLoader.Object);
        await loadDataJob.Execute(_mockJobExecutionContext.Object);

    }
}