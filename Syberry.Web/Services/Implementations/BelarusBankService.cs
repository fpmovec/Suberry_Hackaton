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
        string redisKey = $"{_settings.BankRedisKeys.BelarusBank}_rates";

        IEnumerable<Rate>? ratesData = await _CacheService.GetByKeyAsync<IEnumerable<Rate>>(redisKey);

        if (ratesData == null)
        {
            ratesData = await GetAsync<IEnumerable<Rate>>(_settings.BelarusBankSettings.RatesUrl);
            await _CacheService.UpdateOrCreateAsync(redisKey, ratesData, default);
        }
        
        return ratesData;
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