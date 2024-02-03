using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Syberry.Web.Models;
using Syberry.Web.Services.Abstractions;

namespace Syberry.Web.Services.Implementations;

public class BelarusBankService : IBelarusBankService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppSettings _settings;
    
    public BelarusBankService(IHttpClientFactory httpClientFactory, IOptions<AppSettings> options)
    {
        _httpClientFactory = httpClientFactory;
        _settings = options.Value;
    }
    
    public async Task <List<Rate>> BelarusBankRates()
    {
        var client = _httpClientFactory.CreateClient();
        
        var rates = new List<Rate>();
        
        var pageResponse = await client.GetAsync($"https://belarusbank.by/api/kurs_cards");
        
        var content = await pageResponse.Content.ReadAsStringAsync();

        var jToken = JToken.Parse(content);

        foreach (var item in jToken)
        {
            var rate = new Rate
            {
                KursDateTime = item["kurs_date_time"]!.Value<DateTime>(),
                UsdIn = item["USDCARD_in"]!.Value<double>(),
                UsdOut = item["USDCARD_out"]!.Value<double>(),
                EuroIn = item["EURCARD_in"]!.Value<double>(),
                EuroOut = item["EURCARD_out"]!.Value<double>(),
                RubIn = item["RUBCARD_in"]!.Value<double>(),
                RubOut = item["RUBCARD_out"]!.Value<double>(),
            };
                
            rates.Add(rate);
        }
        
        return rates;
    }
}