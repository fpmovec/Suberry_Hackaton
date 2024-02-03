using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Syberry.Web.Models;
using Syberry.Web.Services.Abstractions;

namespace Syberry.Web.Services.Implementations;

public class BelarusBankService : IBelarusBankService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICacheService _CacheService;
    private readonly AppSettings _settings;
    
    public BelarusBankService(IHttpClientFactory httpClientFactory, IOptions<AppSettings> options, ICacheService cacheService)
    {
        _httpClientFactory = httpClientFactory;
        _CacheService = cacheService;
        _settings = options.Value;
    }
    
    public async Task<IEnumerable<Rate>> GetBelarusBankRatesAsync()
    {
        Bank? cacheData = await _CacheService.GetByKeyAsync<Bank>(_settings.BankRedisKeys.BelarusBank);

        if (cacheData == null)
        {
            cacheData = new Bank
            {
                Name = "Belarusbank",
                Rates = await GetAsync<List<Rate>>(_settings.BelarusBankSettings.RatesUrl)
            };
            await _CacheService.UpdateOrCreateAsync(_settings.BankRedisKeys.BelarusBank, cacheData, default);
        }
        
        return cacheData.Rates;
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