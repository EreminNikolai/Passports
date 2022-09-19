using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData.Interfaces.Storages;
using PassportsUTests.HelperClasses;
using PassportProviders = Passports.Api.Models.Passport.PassportProviders;

namespace PassportsUTests.Models.Passport
{
    [TestFixture]
    public class PassportProviderTests
    {
        private PassportProviders.FileStorage _fileStorage;
        private PassportProviders.DbStorage _dbStorage;
        private PassportProviders.RedisStorage _redisStorage;

        [SetUp]
        public void SetUp()
        {
            _fileStorage = CreateFileStorage();
            _dbStorage = CreateDbStorage();
            _redisStorage = CreateRedisStorage();
        }

        private PassportProviders.DbStorage CreateDbStorage()
        {
            var pair = new KeyValuePair<uint, List<uint>>(1, new List<uint> { 1 });
            var dir = new Dictionary<uint, List<uint>> { [pair.Key] = pair.Value };

            //DbStorage
            var mockDbStorage = new Mock<IDbStorage>();
            mockDbStorage
                .Setup(repo => repo.IsPassportExistAsync(pair.Key, pair.Value[0]).Result)
                .Returns(true);

            var dbStorage = mockDbStorage.Object;

            //Settings
            var mock = new Mock<IOptions<Settings>>();
            mock.Setup(repo => repo.Value).Returns(TestModels.Settings);

            var options = mock.Object;
            
            //Logger
            var mockLogger = new Mock<ILogger<PassportProviders.DbStorage>>();
            
            return new PassportProviders.DbStorage(dbStorage, options, mockLogger.Object);
        }

        private PassportProviders.FileStorage CreateFileStorage()
        {
            var pair = new KeyValuePair<uint, List<uint>>(1, new List<uint> { 1 });
            var dir = new Dictionary<uint, List<uint>> { [pair.Key] = pair.Value };

            //IFileStorage
            var mockFileStorage = new Mock<IFileStorage>();
            mockFileStorage
                .Setup(storage => storage.Create(dir));

            mockFileStorage
                .Setup(storage => storage.IsPassportExistAsync(pair.Key, pair.Value[0]).Result)
                .Returns(true);
            var fileStorage = mockFileStorage.Object;

            //Settings
            var mock = new Mock<IOptions<Settings>>();
            mock.Setup(repo => repo.Value).Returns(TestModels.Settings);

            var options = mock.Object;
            
            //Logger
            var mockLogger = new Mock<ILogger<PassportProviders.FileStorage>>();

            return new PassportProviders.FileStorage(fileStorage, options, mockLogger.Object);
        }

        private PassportProviders.RedisStorage CreateRedisStorage()
        {
            var pair = new KeyValuePair<uint, List<uint>>(1, new List<uint> { 1 });
            var dir = new Dictionary<uint, List<uint>> { [pair.Key] = pair.Value };

            //DbStorage
            var mockRedisStorage = new Mock<IRedisStorage>();
            mockRedisStorage
                .Setup(repo => repo.IsPassportExistAsync(pair.Key, pair.Value[0]).Result)
                .Returns(true);

            var redisStorage = mockRedisStorage.Object;

            //Settings
            var mock = new Mock<IOptions<Settings>>();
            mock.Setup(repo => repo.Value).Returns(TestModels.Settings);

            var options = mock.Object;

            //Logger
            var mockLogger = new Mock<ILogger<PassportProviders.RedisStorage>>();

            return new PassportProviders.RedisStorage(redisStorage, options, mockLogger.Object);
        }


        [TestCase("0","1", ExpectedResult = false, TestName = "Проверка на некорректное значение серии '0'")]
        [TestCase("99999", "1", ExpectedResult = false, TestName = "Проверка на некорректное значение серии '99999'")]
        [TestCase("????", "1", ExpectedResult = false, TestName = "Проверка на некорректное значение серии '????'")]
        [TestCase("", "1", ExpectedResult = false, TestName = "Проверка пустого значения серии")]
        [TestCase("1", "0", ExpectedResult = false, TestName = "Проверка на некорректное значение номера '0'")]
        [TestCase("1", "99999", ExpectedResult = false, TestName = "Проверка на некорректное значение номера '99999'")]
        [TestCase("1", "????", ExpectedResult = false, TestName = "Проверка на некорректное значение номера '????'")]
        [TestCase("1", "", ExpectedResult = false, TestName = "Проверка пустого значения номера")]
        [TestCase("1", "1", ExpectedResult = true, TestName = "Проверка на корректный серию и номер паспорта")]
        public async Task<bool> ExistsCheckingParamsFileStorageTest(string series, string number)
        {
            return await _fileStorage.Exists(series, number);
        }
        
        [TestCase("0", "1", ExpectedResult = false, TestName = "Проверка на некорректное значение серии '0'")]
        [TestCase("99999", "1", ExpectedResult = false, TestName = "Проверка на некорректное значение серии '99999'")]
        [TestCase("????", "1", ExpectedResult = false, TestName = "Проверка на некорректное значение серии '????'")]
        [TestCase("", "1", ExpectedResult = false, TestName = "Проверка пустого значения серии")]
        [TestCase(null, "1", ExpectedResult = false, TestName = "Проверка нула серии")]
        [TestCase("1", "0", ExpectedResult = false, TestName = "Проверка на некорректное значение номера '0'")]
        [TestCase("1", "99999", ExpectedResult = false, TestName = "Проверка на некорректное значение номера '99999'")]
        [TestCase("1", "????", ExpectedResult = false, TestName = "Проверка на некорректное значение номера '????'")]
        [TestCase("1", "", ExpectedResult = false, TestName = "Проверка пустого значения номера")]
        [TestCase("1", null, ExpectedResult = false, TestName = "Проверка нула номера")]
        [TestCase("1", "1", ExpectedResult = true, TestName = "Проверка на корректный серию и номер паспорта")]
        public async Task<bool> ExistsCheckingParamsRedisStorageTest(string series, string number)
        {
            return await _redisStorage.Exists(series, number);
        }

        [TestCase("0", "1", ExpectedResult = false, TestName = "Проверка на некорректное значение серии '0'")]
        [TestCase("99999", "1", ExpectedResult = false, TestName = "Проверка на некорректное значение серии '99999'")]
        [TestCase("????", "1", ExpectedResult = false, TestName = "Проверка на некорректное значение серии '????'")]
        [TestCase("", "1", ExpectedResult = false, TestName = "Проверка пустого значения серии")]
        [TestCase("1", "0", ExpectedResult = false, TestName = "Проверка на некорректное значение номера '0'")]
        [TestCase("1", "99999", ExpectedResult = false, TestName = "Проверка на некорректное значение номера '99999'")]
        [TestCase("1", "????", ExpectedResult = false, TestName = "Проверка на некорректное значение номера '????'")]
        [TestCase("1", "", ExpectedResult = false, TestName = "Проверка пустого значения номера")]
        [TestCase("1", "1", ExpectedResult = true, TestName = "Проверка на корректный серию и номер паспорта")]
        public async Task<bool> ExistsCheckingParamsDbStorageTest(string series, string number)
        {
            return await _dbStorage.Exists(series, number);
        }

    }
}