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

    private readonly INationalBankService _nationalBank;


    public RatesController(IBelarusBankService belarusBankService, IAlpfaBankService alpfaBankService,
        INationalBankService nationalBank)
    {
        _alpfaBankService = alpfaBankService;
        _nationalBank = nationalBank;
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

        var nRate = await _nationalBank.GetNationalBankAsync();
        
        res.Add(nRate);
        
        return Ok(res);
    }
}