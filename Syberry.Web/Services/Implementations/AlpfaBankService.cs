using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Syberry.Web.Models;
using Syberry.Web.Models.Dto;
using Syberry.Web.Services.Abstractions;

namespace Syberry.Web.Services.Implementations;

public class AlpfaBankService : IAlpfaBankService
{
    private readonly HttpClient _client;
    private readonly AppSettings _settings;
    //private readonly ICacheService _cacheService;
    
    public AlpfaBankService(IOptions<AppSettings> settings, HttpClient client
        //ICacheService cacheService
        )
    {
        _client = client;
        //_cacheService = cacheService;
        _settings = settings.Value;
    }
    
    public async Task <Bank> AlpfaBankRates()
    {

        //Bank? cacheData = await _cacheService.GetByKeyAsync<Bank>(_settings.BankRedisKeys.AlphaBank);

        //if (cacheData is null)
        //{
            var alpfaRates = new List<AlpfabankRateDto>();
        
            var pageResponse = await _client.GetAsync(_settings.AlphaBankSettings.RatesUrl);
        
            var content = await pageResponse.Content.ReadAsStringAsync();

            var jToken = JToken.Parse(content);
        
            var pageItems = (JArray)jToken["rates"];

            foreach (var item in pageItems)
            {
                var alpfaRate = new AlpfabankRateDto
                {
                    SellRate = item["sellRate"]!.Value<double>(),
                    SellIso = item["sellIso"]!.Value<string>(),
                    BuyRate = item["buyRate"]!.Value<double>(),
                    BuyIso = item["buyIso"]!.Value<string>(),
                    Date = item["date"]!.Value<DateTime>(),
                };
                
                alpfaRates.Add(alpfaRate);
            }

            var cacheData = new Bank
            {
                Name = "AlpfaBank",
                Rates = alpfaRates.Where(x => x.BuyIso == "BYN")
                    .Select(x => new Rate
                    {
                        Name = x.SellIso,
                        SellRate = x.SellRate,
                        BuyRate = x.BuyRate,
                        KursDateTime = x.Date
                    }).ToList()
            };
        //}
        
        
        return cacheData;
    }

    public async Task<CurrencyView> GetCurrencies()
    {
        var alpfaRates = new List<AlpfabankRateDto>();
        
        var pageResponse = await _client.GetAsync(_settings.AlphaBankSettings.RatesUrl);
        
        var content = await pageResponse.Content.ReadAsStringAsync();

        var jToken = JToken.Parse(content);
        
        var pageItems = (JArray)jToken["rates"];

        foreach (var item in pageItems)
        {
            var alpfaRate = new AlpfabankRateDto
            {
                SellRate = item["sellRate"]!.Value<double>(),
                SellIso = item["sellIso"]!.Value<string>(),
                BuyRate = item["buyRate"]!.Value<double>(),
                BuyIso = item["buyIso"]!.Value<string>(),
                Date = item["date"]!.Value<DateTime>(),
            };
                
            alpfaRates.Add(alpfaRate);
        }

        return new CurrencyView()
        {
            BankName = "Alphabank",
            Currencies = alpfaRates.Select(x => x.SellIso).Distinct().ToList()
        };
    }
}