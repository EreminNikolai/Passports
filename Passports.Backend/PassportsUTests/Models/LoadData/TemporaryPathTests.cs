using System;
using System.IO;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData;
using PassportsUTests.HelperClasses;

namespace PassportsUTests.Models.LoadData
{
    [TestFixture]
    public class TemporaryPathTests
    {
        [Test]
        public void TemporaryPathTest()
        {
            var mockSettings = new Mock<IOptions<Settings>>();
            mockSettings.Setup(repo => repo.Value).Returns(TestModels.Settings);

            var mockRepositoryPath = new Mock<TemporaryPath>(MockBehavior.Default, mockSettings.Object);
            mockRepositoryPath
                .Setup(path => path.CreateDirectory());

            var repositoryPath = mockRepositoryPath.Object;

            
            Assert.AreEqual(Path.Combine(Environment.CurrentDirectory, TestModels.Settings.NameDirectoryData), repositoryPath.DataPath);
            Assert.AreEqual(Path.Combine(Environment.CurrentDirectory, TestModels.Settings.NameDirectoryData, "Data.csv"), repositoryPath.DataFilePath);

            var nameDirectory = "TEST";
            var result = repositoryPath.GetFullPath(nameDirectory);
            var testPath = Path.Combine(Environment.CurrentDirectory, TestModels.Settings.NameDirectoryData, nameDirectory);
            Assert.AreEqual(testPath, result);
        }
    }
}