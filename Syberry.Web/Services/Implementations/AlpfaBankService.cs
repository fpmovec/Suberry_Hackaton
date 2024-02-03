using Newtonsoft.Json.Linq;
using Syberry.Web.Models;
using Syberry.Web.Models.Dto;
using Syberry.Web.Services.Abstractions;

namespace Syberry.Web.Services.Implementations;

public class AlpfaBankService : IAlpfaBankService
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    public AlpfaBankService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task <Bank> AlpfaBankRates()
    {
        var client = _httpClientFactory.CreateClient();
        
        var alpfaRates = new List<AlpfabankRateDto>();
        
        var pageResponse = await client.GetAsync($"https://developerhub.alfabank.by:8273/partner/1.0.1/public/rates");
        
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

        var res = new Bank
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
        
        return res;
    }
}