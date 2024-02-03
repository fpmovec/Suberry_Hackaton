using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Syberry.Web.Models;
using Syberry.Web.Services.Abstractions;

namespace Syberry.Web.Services.Implementations;

public class BelarusBankService : IBelarusBankService
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    public BelarusBankService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
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
                UsdIn = item["kurs_date_time"]!.Value<double>(),
                UsdOut = item["kurs_date_time"]!.Value<double>(),
                EuroIn = item["kurs_date_time"]!.Value<double>(),
                EuroOut = item["kurs_date_time"]!.Value<double>(),
                RubIn = item["kurs_date_time"]!.Value<double>(),
                RubOut = item["kurs_date_time"]!.Value<double>(),
            };
                
            rates.Add(rate);
        }
        
        return rates;
    }
}