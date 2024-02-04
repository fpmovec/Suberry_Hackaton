using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Syberry.Web.Models;
using Syberry.Web.Models.Dto;
using Syberry.Web.Services.Abstractions;

namespace Syberry.Web.Services.Implementations;

public class NationalBankService : INationalBankService
{
    private readonly HttpClient _client;
    //private readonly ICacheService _cacheService;
    private readonly AppSettings _settings;

    public NationalBankService(HttpClient client,
        //ICacheService cacheService,
        IOptions<AppSettings> options)
    {
        _client = client;
        //_cacheService = cacheService;
        _settings = options.Value;
    }


    public async Task<Bank> GetNationalBankAsync()
    {
        //    Bank? cacheData = await _cacheService.GetByKeyAsync<Bank>(_settings.BankRedisKeys.NationalBank);

        //    if (cacheData is null)
        //    {
        var nationalRates = new List<NationalBankDto>();
                    
            var rates = new List<Rate>();
                    
            var pageResponse = await _client.GetAsync("https://api.nbrb.by/exrates/rates?periodicity=0");
                    
            var content = await pageResponse.Content.ReadAsStringAsync();
            
            var jToken = JToken.Parse(content);

            foreach (var item in jToken)
            {
                        var rate = new NationalBankDto()
                        {
                            Date = item["Date"]!.Value<DateTime>(),
                            Rate = item["Cur_OfficialRate"]!.Value<double>(),
                            Name = item["Cur_Abbreviation"]!.Value<string>()
                        };
                        nationalRates.Add(rate);
            }

            nationalRates = nationalRates.Where(i => i.Name is "USD" or "EUR" or "RUB").ToList();
            foreach (var x in nationalRates)
            {
                        var rate = new Rate
                        {
                            Name = x.Name,
                            BuyRate = default,
                            SellRate = x.Rate,
                            KursDateTime = x.Date
                        };
                        
                        rates.Add(rate);
            }

            var cacheData = new Bank()
            {
                Name = "National Bank",
                Rates = rates
            };
        //}

        return cacheData;
    }


    public async Task<CurrencyView> GetCurrencies()
    {
        
        var nationalRates = new List<NationalBankDto>();
                    
        var rates = new List<Rate>();
                    
        var pageResponse = await _client.GetAsync(_settings.NationalBankSettings.RatesUrl);
                    
        var content = await pageResponse.Content.ReadAsStringAsync();
            
        var jToken = JToken.Parse(content);

        foreach (var item in jToken)
        {
            var rate = new NationalBankDto()
            {
                Name = item["Cur_Abbreviation"]!.Value<string>()
            };
            nationalRates.Add(rate);
        }

        return new CurrencyView()
        {
            BankName = "Nationalbank",
            Currencies = nationalRates.Select(c => c.Name).ToList()
        };
    }
}