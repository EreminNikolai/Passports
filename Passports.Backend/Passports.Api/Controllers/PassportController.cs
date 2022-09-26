using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Passports.Api.Models.LoadData.Interfaces;
using Passports.Api.Models.Passport.Interfaces;

namespace Passports.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PassportController : ControllerBase
{
    private readonly IPassportProvider _passport;
    private readonly ILoader _loader;

    public PassportController(IPassportProvider passport, ILoader loader)
    {
        _passport = passport;
        _loader = loader;
    }

    /// <summary>
    /// Проверка наличия паспорта
    /// </summary>
    /// <param name="series">Серия паспорта</param>
    /// <param name="number">Номер паспорта</param>
    /// <returns></returns>
    [HttpGet("{series}/{number}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<bool> ExistPassport(string series, string number)
    {
        return await _passport.Exists(series, number).ConfigureAwait(false); ;
    }

    [HttpPost]
    public async Task LoadData()
    {
        await _loader.LoadAsync();
    }
}