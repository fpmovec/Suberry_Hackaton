using Microsoft.AspNetCore.Mvc;

namespace Syberry.Web.Controllers;

[ApiController]
[Route("/api")]
public class InfoController : ControllerBase
{
    [HttpGet("/banks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetBanksList()
        => Ok(Constants.BanksLists);

    [HttpGet("/banks/{bankName:bank}/currencies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetCurrenciesByBankName([FromRoute] string bankName)
    {
        //TODO: add logic
        
        return Ok();
    }
}