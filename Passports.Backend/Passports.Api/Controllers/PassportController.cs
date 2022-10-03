using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Passports.Api.Models.Dto;
using Passports.Api.Models.LoadData.Interfaces;
using Passports.Api.Models.Passport;
using Passports.Api.Models.Passport.Interfaces;

namespace Passports.Api.Controllers;

[ApiVersion("1.0")]
[ApiVersion("2.0")]
//[ApiVersionNeutral]
[Produces("application/json")]
[Route("api/{version:apiVersion}/[controller]")]
[ApiController]
public class PassportController : BaseController
{
    private readonly IPassportProvider _passport;
    private readonly ILoader _loader;
    private readonly IMapper _mapper;

    public PassportController(IPassportProvider passport, ILoader loader, IMapper mapper)
    {
        _passport = passport;
        _loader = loader;
        _mapper = mapper;
    }

    /// <summary>
    /// Проверка наличия паспорта
    /// </summary>
    /// <remarks>
    /// Пример запроса определения паспорта Get /passports
    /// </remarks>
    /// <param name="series">Серия паспорта</param>
    /// <param name="number">Номер паспорта</param>
    /// <returns>Возвращает true/false</returns>
    /// <response code="200">Success</response>
    /// /// <response code="401">Success</response>
    [HttpGet("{series}/{number}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<bool> ExistPassport(string series, string number)
    {
        return await _passport.Exists(series, number).ConfigureAwait(false); ;
    }

    [HttpPost("LoadData")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task LoadData()
    {
        await _loader.LoadAsync();
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<PassportDto>>> GetAll(int skip, int top)
    {
        var passports = await _passport.GetAll(skip, top);
        return Ok(passports);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreatePassportDto createPassportDto)
    {
        var passport = _mapper.Map<Passport>(createPassportDto);
        passport.UserId = UserId;
        var passportId = await _passport.Create(passport);
        return Ok(passportId);
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update([FromBody] UpdatePassportDto updatePassportDto)
    {
        var passport = _mapper.Map<Passport>(updatePassportDto);
        passport.UserId = UserId;
        await _passport.Update(passport);
        return NoContent();
    }
}