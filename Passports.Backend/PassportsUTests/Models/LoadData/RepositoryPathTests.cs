using System;
using System.IO;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData;
using PassportsUTests.HelperClasses;

namespace PassportsUTests.Models.LoadData;

[TestFixture]
public class RepositoryPathTests
{

    [Test]
    public void RepositoryPathTest()
    {
        var mockSettings = new Mock<IOptions<Settings>>();
        mockSettings.Setup(repo => repo.Value).Returns(TestModels.Settings);

        var mockRepositoryPath = new Mock<RepositoryPath>(MockBehavior.Default, mockSettings.Object);
        mockRepositoryPath
            .Setup(path => path.CreateDirectory());

        var nameDirectory = "TEST";
        var result = mockRepositoryPath.Object.GetDirectoryRepositoryPath(nameDirectory);
        var testPath = Path.Combine(Environment.CurrentDirectory, TestModels.Settings.NameDirectoryRepository, nameDirectory);
        Assert.AreEqual(testPath,result);
        Assert.AreEqual(Path.Combine(Environment.CurrentDirectory, TestModels.Settings.NameDirectoryRepository),mockRepositoryPath.Object.DirectoryRepositoryPath);
    }
        
}