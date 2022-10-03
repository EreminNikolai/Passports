using Passports.Api.Models.Dto;

namespace Passports.Api.Models.Passport.Interfaces;

public interface IPassportProvider
{
    Task<bool> Exists(string series, string number);
    Task<int> Create(Passport passport);
    Task Update(Passport passport);
    Task<List<PassportDto>> GetAll(int skip, int top);
}