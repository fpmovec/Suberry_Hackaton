using Microsoft.AspNetCore.Mvc;
using Syberry.Web.Services.Abstractions;

namespace Syberry.Web.Controllers;

[ApiController]
[Route("/api")]
public class InfoController(
    INationalBankService _nationalBankService,
    IAlpfaBankService _alpfaBankService,
    IBelarusBankService _belarusBankService
    ) : ControllerBase
{
    [HttpGet("/banks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetBanksList()
        => Ok(Constants.BanksLists);

    [HttpGet("/banks/{bankName:bank}/currencies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrenciesByBankName([FromRoute] string bankName)
    {
        switch (bankName.ToLower())
        {
            case "national":
                return Ok(await _nationalBankService.GetCurrencies());
            case "belarusbank":
                return Ok(_belarusBankService.GetCurrencies());
            case "alphabank":
                return Ok(await _alpfaBankService.GetCurrencies());
        }

        return BadRequest();
    }
}