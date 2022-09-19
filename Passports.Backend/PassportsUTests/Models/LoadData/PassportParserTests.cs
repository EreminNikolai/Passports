using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Passports.Api.Helpers;
using Passports.Api.Models.LoadData;
using PassportsUTests.HelperClasses;

namespace PassportsUTests.Models.LoadData
{
    [TestFixture]
    public class PassportParserTests
    {
        [Test]
        public async Task ParseTest()
        {
            var mockSettings = new Mock<IOptions<Settings>>();
            mockSettings.Setup(repo => repo.Value).Returns(TestModels.Settings);

            var mockPassportParser = new Mock<PassportParser>(MockBehavior.Default, mockSettings.Object);
            mockPassportParser
                .Setup(parser => parser.FileExists(It.IsAny<string>()))
                .Returns(true);
            uint key1 = 1111;
            uint key2 = 2222;
            var dirTest = new Dictionary<uint, List<uint>>
            {
                [key1] = new List<uint> { 111111, 222222 },
                [key2] = new List<uint> { 111111 }
            };
            mockPassportParser
                .Setup(parser => parser.FileReadLines(It.IsAny<string>()))
                .Returns(new List<string>
                {
                    $"{key1},{dirTest[key1][0]}",
                    $"{key1},{dirTest[key1][1]}",
                    $"{key2},{dirTest[key2][0]}",
                    "1qw1,111111",
                    "1111,11qw11",
                    "1111#111111",
                    $"1111{It.IsAny<string>()}111111",
                    "1111",
                    ",1111",
                    It.IsAny<string>(),
                    string.Empty,
                    null
                });
            var result = await mockPassportParser.Object.Parse(It.IsAny<string>()).ConfigureAwait(false);
            CollectionAssert.AreEqual(dirTest, result);
        }
    }
}