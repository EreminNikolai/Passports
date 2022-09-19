using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Passports.Api.Models.Passport.Interfaces;

namespace Passports.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PassportController : ControllerBase
{
    private readonly IPassportProvider _passport;
    public PassportController(IPassportProvider passport)
    {
        _passport = passport;
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

    [HttpGet]
    public string Get()
    {
        return "HELLO WORLD!!! WELCOME TO HELL!!! 123";
    }
}