using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syberry.Telegram
{
    internal class ApiService
    {
        private const string BaseQueryAddress =
            "https://localhost:7129/";

        private static readonly HttpClient _httpClient = new();

        public static async Task<CurrencyBelarusRatesDto> GetTitle(string titleName)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{BaseQueryAddress}");

            string jsonInfo = await response.Content.ReadAsStringAsync();

            CurrencyBelarusRatesDto res = JsonConvert.DeserializeObject<CurrencyBelarusRatesDto>(jsonInfo);

            return res;
        }
    }
}
