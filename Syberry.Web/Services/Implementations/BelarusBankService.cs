using Microsoft.Extensions.Options;
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
    
    public async Task<IEnumerable<Rate>> GetBelarusBankRatesAsync()
    {
        var data = await GetAsync<IEnumerable<Rate>>(_settings.BelarusBankSettings.RatesUrl);

        return data;
    }


    private async Task PostAsync<T>(string url, T value){ }

    private async Task<T?> GetAsync<T>(string url)
    {
        var client = _httpClientFactory.CreateClient();
        var responseStringContent = await client.GetAsync(url).Result
            .Content.ReadAsStringAsync();

        var data = JToken.Parse(responseStringContent).ToObject<T>();

        return data;
    } 
    
}