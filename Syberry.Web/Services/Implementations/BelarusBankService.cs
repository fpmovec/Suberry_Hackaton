using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Syberry.Web.Models;
using Syberry.Web.Models.Dto;
using Syberry.Web.Services.Abstractions;

namespace Syberry.Web.Services.Implementations;

public class BelarusBankService : IBelarusBankService
{
    //private readonly ICacheService _CacheService;
    private readonly AppSettings _settings;
    private readonly HttpClient _client = new();
    
    public BelarusBankService(IOptions<AppSettings> options
        //ICacheService cacheService,
    )
    {
        //_CacheService = cacheService;
        _settings = options.Value;
    }
    
    public async Task<Bank> BelarusBankRates()
    {
        //Bank? cacheData = await _CacheService.GetByKeyAsync<Bank>(_settings.BankRedisKeys.BelarusBank);
        //if (cacheData == null)
        //{
            var belarusRates = new List<BelarusBankDto>();
                    
                    var rates = new List<Rate>();
                    
                    var pageResponse = await _client.GetAsync("https://belarusbank.by/api/kurs_cards");


                    var content = await pageResponse.Content.ReadAsStringAsync();
            
                    var jToken = JToken.Parse(content);
            
                    foreach (var item in jToken)
                    {
                        var rate = new BelarusBankDto()
                        {
                            KursDateTime = item["kurs_date_time"]!.Value<DateTime>(),
                            UsdIn = item["USDCARD_in"]!.Value<double>(),
                            UsdOut = item["USDCARD_out"]!.Value<double>(),
                            EurIn = item["EURCARD_in"]!.Value<double>(),
                            EurOut = item["EURCARD_out"]!.Value<double>(),
                            RubIn = item["RUBCARD_in"]!.Value<double>(),
                            RubOut = item["RUBCARD_out"]!.Value<double>(),
                        };
                            
                        belarusRates.Add(rate);
                    }
            
                    foreach (var x in belarusRates)
                    {
                        var usd = new Rate
                        {
                            Name = "USD",
                            BuyRate = x.UsdOut,
                            SellRate = x.UsdIn,
                            KursDateTime = x.KursDateTime
                        };
                        
                        rates.Add(usd);
                        
                        var rub = new Rate
                        {
                            Name = "RUB",
                            BuyRate = x.RubOut,
                            SellRate = x.RubIn,
                            KursDateTime = x.KursDateTime
                        };
                        
                        rates.Add(rub);
                        
                        var eur = new Rate
                        {
                            Name = "EUR",
                            BuyRate = x.EurOut,
                            SellRate = x.EurIn,
                            KursDateTime = x.KursDateTime
                        };
                        
                        rates.Add(eur);
                    }

        var cacheData = new Bank
        {
            Name = "BelarusBank",
            Rates = rates
        };

        //            await _CacheService.UpdateOrCreateAsync(_settings.BankRedisKeys.BelarusBank, cacheData, default);
        //}

        return cacheData;
    }

    public CurrencyView GetCurrencies()
    {
        return new CurrencyView()
        {
            BankName = "Belarusbank",
            Currencies = ["USD", "EUR", "RUB"]
        };
    }
}