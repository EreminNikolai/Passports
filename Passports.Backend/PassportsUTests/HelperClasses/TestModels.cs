using Passports.Api.Helpers;

namespace PassportsUTests.HelperClasses;

public static class TestModels
{
    private static Settings _settings;
    public static Settings Settings => _settings ??= new Settings
    {
        Url = "http://guvm.mvd.ru/upload/expired-passports/list_of_expired_passports.csv.bz2",
        DownloadCronStartTime = "0 0 15 ? * *",
        MaxSeries = 9999,
        MaxNumber = 999999,
        NameDirectoryData = "Data",
        NameDirectoryRepository = "Repository"
    };
}