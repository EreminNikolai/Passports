namespace Passports.Api.Models.Passport.Interfaces;

public interface IPassportProvider
{
    Task<bool> Exists(string series, string number);
}