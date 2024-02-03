using Microsoft.AspNetCore.Mvc;
using Syberry.Web.Services.Abstractions;
using Syberry.Web.Services.Implementations;

namespace Syberry.Web.Controllers;

[Route("api/v1/rates")]
public class RatesController : ControllerBase
{
    private readonly IBelarusBankService _belarusBankService;

    public RatesController(
        IHttpClientFactory httpClientFactory,
        IBelarusBankService belarusBankService)
    {
        _belarusBankService = belarusBankService;
    }
    
    [HttpGet]
    public async Task<IActionResult> ParseRates()
    {
        var bRate = await _belarusBankService.BelarusBankRates();
        
        return Ok(bRate);
    }
}