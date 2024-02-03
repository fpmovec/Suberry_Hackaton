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
    
    public async Task<Rate> BelarusBankRates()
    {
        var client = _httpClientFactory.CreateClient();
        
        var pageResponse = await client.GetAsync($"https://belarusbank.by/api/kurs_cards");
        
        var content = await pageResponse.Content.ReadAsStringAsync();

        var jToken = JToken.Parse(content);

        var rate = jToken.ToObject<Rate>();
        
        return rate;
    }
}