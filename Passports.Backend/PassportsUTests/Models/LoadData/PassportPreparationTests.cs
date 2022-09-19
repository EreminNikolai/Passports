using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Passports.Api.Models.LoadData;
using Passports.Api.Models.LoadData.Interfaces;

namespace PassportsUTests.Models.LoadData;

[TestFixture]
public class PassportPreparationTests
{
    [Test]
    public async Task PreparationFromFileAsyncTest()
    {
        var dirTest = new Dictionary<uint, List<uint>> { [1111] = new List<uint> { 111111 } };
        var repository = new MockRepository(MockBehavior.Strict)
        {
            CallBase = true,
            DefaultValue = DefaultValue.Mock
        };
        var mockSequence = new MockSequence();
        var mockArchiver = repository.Create<IArchiver>();
        mockArchiver
            .InSequence(mockSequence)
            .Setup(archiver => archiver.Decompress("fileName", It.IsAny<bool>()).Result)
            .Returns("fileResult");

        var mockPassportParser = repository.Create<IPassportParser>();
        mockPassportParser
            .InSequence(mockSequence)
            .Setup(parser => parser.Parse("fileResult").Result)
            .Returns(dirTest);

        var mockTempPath = repository.Create<ITemporaryPath>();
        mockTempPath
            .InSequence(mockSequence)
            .Setup(path => path.DeleteDataFile());


        var passportPreparation = new PassportPreparation(mockArchiver.Object, mockPassportParser.Object, mockTempPath.Object);
        var result = await passportPreparation.PreparationFromFileAsync("fileName");
        CollectionAssert.AreEqual(dirTest, result);

        mockArchiver.Verify(archiver => archiver.Decompress(It.IsAny<string>(), It.IsAny<bool>()).Result, Times.AtLeastOnce);
        mockPassportParser.Verify(parser => parser.Parse(It.IsAny<string>()).Result, Times.AtLeastOnce);
        mockTempPath.Verify(path => path.DeleteDataFile(), Times.AtLeastOnce);
        repository.VerifyAll();
    }
}