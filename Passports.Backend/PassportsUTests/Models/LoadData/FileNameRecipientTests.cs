using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData;
using PassportsUTests.HelperClasses;

namespace PassportsUTests.Models.LoadData;

[TestFixture]
public class FileNameRecipientTests
{
    /// <summary>
    /// Проверяем на получение корректного значения метода GetFileNameFromURL
    /// </summary>
    [Test]
    public void GetFileNameFromURLCheckCorrectValueTest()
    {
        // Arrange
        var mockSettings = new Mock<IOptions<Settings>>();
        mockSettings.Setup(options => options.Value).Returns(TestModels.Settings);
        var settings = mockSettings.Object;

        var fileLoader = new FileNameRecipient(settings);

        // Act
        var testPath = Path.GetFileName(TestModels.Settings.Url);
        var result = fileLoader.GetFileNameFromURL().Result;

        // Assert
        Assert.AreEqual(testPath, result);
    }

    /// <summary>
    /// Проверяем, возникает ли ошибка, если URL некорректный
    /// </summary>
    [Test]
    public void GetFileNameFromURLTestingIncorrectUrlTest()
    {
        // Arrange
            
        var mockSettings = new Mock<IOptions<Settings>>();
        mockSettings
            .Setup(options => options.Value)
            .Returns(new Settings { Url = It.IsAny<string>() });
            
        var settings = mockSettings.Object;

        var mockFileNameRecipient = new Mock<FileNameRecipient>(settings);
        mockFileNameRecipient
            .Setup(e => e.PathGetFileName(It.IsAny<string>()))
            .Returns("");

        var fileLoader = mockFileNameRecipient.Object;

        // Act
        Task Handler() => fileLoader.GetFileNameFromURL();
            
        // Assert
        Assert.CatchAsync(typeof(Exception), Handler, "Ошибка, если URL некорректный.");
    }
}