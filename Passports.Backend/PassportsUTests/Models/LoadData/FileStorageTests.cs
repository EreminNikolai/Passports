using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData.Interfaces;
using Passports.Api.Models.LoadData.Storages;
using PassportsUTests.HelperClasses;

namespace PassportsUTests.Models.LoadData;

[TestFixture]
public class FileStorageTests
{
    [Test]
    public void CreateTest()
    {
        // Arrange
        var pair = new KeyValuePair<uint, List<uint>>(1111, new List<uint> { 111111 });
        var dir = new Dictionary<uint, List<uint>> { [pair.Key] = pair.Value };

        var mockRepositoryPath = new Mock<IRepositoryPath>();
        mockRepositoryPath
            .Setup(path => path.GetDirectoryRepositoryPath(""))
            .Returns("");
        mockRepositoryPath
            .Setup(path => path.DirectoryRepositoryPath)
            .Returns("");

        var mockSettings = new Mock<IOptions<Settings>>();
        mockSettings.Setup(repo => repo.Value).Returns(TestModels.Settings);

        var mockFileStorage = new Mock<FileStorage>(MockBehavior.Default, mockRepositoryPath.Object, mockSettings.Object);
        mockFileStorage
            .Setup(e => e.CreateFile(pair.Key.ToString(), pair.Value))
            .Returns(Task.CompletedTask);

        mockFileStorage.Object.Create(dir);
        mockFileStorage.Verify(e => e.CreateFile(pair.Key.ToString(), pair.Value), Times.Once, "Метод CreateFile вызывается несколько раз");
        mockFileStorage.Verify(e => e.CreateFile("pair.Key.ToString()", new List<uint>()), Times.Never, "Метод CreateFile вообще не должен вызываться");


        var mockFileStorageError = new Mock<FileStorage>(MockBehavior.Default, mockRepositoryPath.Object, mockSettings.Object);
        mockFileStorageError
            .Setup(e => e.CreateFile(pair.Key.ToString(), pair.Value))
            .Returns(() => throw new Exception("Возможная ошибка."));

        Assert.CatchAsync(() => mockFileStorageError.Object.Create(dir),"Если возникает ошибка в методе, она должна быть обработана");
    }

    [Test]
    public async Task IsPassportExistAsyncTest()
    {
        // Arrange
        var pair = new KeyValuePair<uint, List<uint>>(1111, new List<uint> { 111111 });
        var dir = new Dictionary<uint, List<uint>> { [pair.Key] = pair.Value };

        var mockRepositoryPath = new Mock<IRepositoryPath>();
        mockRepositoryPath
            .Setup(path => path.GetDirectoryRepositoryPath(""))
            .Returns("");
        mockRepositoryPath
            .Setup(path => path.DirectoryRepositoryPath)
            .Returns("");

        var mockSettings = new Mock<IOptions<Settings>>();
        mockSettings.Setup(repo => repo.Value).Returns(TestModels.Settings);


        var mockFileStorage = new Mock<FileStorage>(MockBehavior.Default, mockRepositoryPath.Object, mockSettings.Object);
        mockFileStorage
            .Setup(e => e.ReadNumberFile(It.IsAny<string>(), 1))
            .Returns(true);

        var result = await mockFileStorage.Object.IsPassportExistAsync(111, 1).ConfigureAwait(false);
        Assert.IsTrue(result, "Не корректный результат. Паспорт с номером 1 должен быть найден");
        mockFileStorage.Verify(e => e.ReadNumberFile(It.IsAny<string>(), It.IsAny<uint>()), Times.Once);

        //Общая проверка тела метода.
        result = await mockFileStorage.Object.IsPassportExistAsync(111, 1111).ConfigureAwait(false);
        Assert.IsFalse(result, "Не корректный результат. Паспорт с номером 1111 нет");

        //Проверка при Exceptions
        var mockFileStorageError = new Mock<FileStorage>(MockBehavior.Default, mockRepositoryPath.Object, mockSettings.Object);
        mockFileStorageError
            .Setup(e => e.ReadNumberFile(It.IsAny<string>(), It.IsAny<uint>()))
            .Returns(() => throw new Exception("Возможная ошибка."));

        //Проверяем, что в методе нет никаких exceptions
        Assert.DoesNotThrowAsync(async () => await mockFileStorageError.Object.IsPassportExistAsync(1, 1), "Ошибки должны быть обработаны.");

        //Проверяем, что если в методе возникает ошибка, то возвращаем false
        result = await mockFileStorageError.Object.IsPassportExistAsync(111, 1111).ConfigureAwait(false);
        Assert.IsFalse(result, "Если возникает ошибка в методе, то в результате должны получать False");
    }
}