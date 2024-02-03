﻿using Microsoft.AspNetCore.Mvc;
using Syberry.Web.Models;
using Syberry.Web.Models.Dto;
using Syberry.Web.Services.Abstractions;

namespace Syberry.Web.Controllers;

[ApiController]
[Route("/api")]
public class InfoController : ControllerBase
{
    private readonly IBelarusBankService _belarusBankService;
    private readonly IAlpfaBankService _alpfaBankService;

    public InfoController(
        IBelarusBankService belarusBankService,
        IAlpfaBankService alpfaBankService)
    {
        _alpfaBankService = alpfaBankService;
        _belarusBankService = belarusBankService;
    }
    
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
    
    [HttpGet("/Rate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Task3 ([FromRoute] string currencyCode, string bankName, DateTime date)
    {
        var res = new List<Bank>();
        
        var bRate = await _belarusBankService.BelarusBankRates();
        
        res.Add(bRate);
        
        var aRate = await _alpfaBankService.AlpfaBankRates();
        
        res.Add(aRate);

        var item = res.Where(x => x.Name == bankName 
                                  && x.Rates.Any(x => x.KursDateTime == date) 
                                  && x.Rates.Any(x => x.Name == currencyCode));
        
        return Ok(item);
    }
    
    [HttpGet("/Rate/rates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Task4 ([FromRoute] string currencyCode, string bankName, DateTime from, DateTime to)
    {
        var res = new List<Bank>();
        
        var bRate = await _belarusBankService.BelarusBankRates();
        
        res.Add(bRate);
        
        var aRate = await _alpfaBankService.AlpfaBankRates();
        
        res.Add(aRate);

        var item = res.Where(x => x.Name == bankName 
                                  && x.Rates.Any(x => x.KursDateTime <= to 
                                                        && x.KursDateTime >= from) 
                                  && x.Rates.Any(x => x.Name == currencyCode));
        
        return Ok(item);
    }
    
    /*[HttpGet("/Rate/rates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Task5 ([FromRoute] string currencyCode, string bankName, DateTime from, DateTime to)
    {
        var res = new List<Bank>();
        
        var bRate = await _belarusBankService.BelarusBankRates();
        
        res.Add(bRate);
        
        var aRate = await _alpfaBankService.AlpfaBankRates();
        
        res.Add(aRate);

        var list = res.Where(x => x.Name == bankName 
                                  && x.Rates.Any(x => x.KursDateTime <= to 
                                                      && x.KursDateTime >= from) 
                                  && x.Rates.Any(x => x.Name == currencyCode))
            .ToList();
        
        var stat = new StatisticsDto
        {
            AverageRate = list.,
            ChartImage = ,
            MaxRate = ,
            MinRate = ,
            RateAtThePeriodend = ,
            RateAtThePeriodStart = 
        }
        
        return Ok();
    }*/
}