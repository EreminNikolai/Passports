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
        private PassportProviders.FileStorageProvider _fileStorageProvider;
        private PassportProviders.DbStorageProvider _dbStorageProvider;
        private PassportProviders.RedisStorageProvider _redisStorageProvider;

        [SetUp]
        public void SetUp()
        {
            _fileStorageProvider = CreateFileStorage();
            _dbStorageProvider = CreateDbStorage();
            _redisStorageProvider = CreateRedisStorage();
        }

        private PassportProviders.DbStorageProvider CreateDbStorage()
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
            var mockLogger = new Mock<ILogger<PassportProviders.DbStorageProvider>>();
            
            return new PassportProviders.DbStorageProvider(dbStorage, options, mockLogger.Object);
        }

        private PassportProviders.FileStorageProvider CreateFileStorage()
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
            var mockLogger = new Mock<ILogger<PassportProviders.FileStorageProvider>>();

            return new PassportProviders.FileStorageProvider(fileStorage, options, mockLogger.Object);
        }

        private PassportProviders.RedisStorageProvider CreateRedisStorage()
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
            var mockLogger = new Mock<ILogger<PassportProviders.RedisStorageProvider>>();

            return new PassportProviders.RedisStorageProvider(redisStorage, options, mockLogger.Object);
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
            return await _fileStorageProvider.Exists(series, number);
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
            return await _redisStorageProvider.Exists(series, number);
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
            return await _dbStorageProvider.Exists(series, number);
        }

    }
}