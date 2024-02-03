using Microsoft.AspNetCore.Mvc;
using Syberry.Web.Models;
using Syberry.Web.Services.Abstractions;
using Syberry.Web.Services.Implementations;

namespace Syberry.Web.Controllers;

[Route("api/v1/rates")]
public class RatesController : ControllerBase
{
    private readonly IBelarusBankService _belarusBankService;
    private readonly IAlpfaBankService _alpfaBankService;

    public RatesController(
        IBelarusBankService belarusBankService,
        IAlpfaBankService alpfaBankService)
    {
        _alpfaBankService = alpfaBankService;
        _belarusBankService = belarusBankService;
    }
    
    [HttpGet]
    public async Task<IActionResult> ParseRates()
    {
        var res = new List<Bank>();
        
        var bRate = await _belarusBankService.BelarusBankRates();
        
        res.Add(bRate);
        
        var aRate = await _alpfaBankService.AlpfaBankRates();
        
        res.Add(aRate);
        
        return Ok(res);
    }
}