using Newtonsoft.Json;

namespace Syberry.Telegram
{
    internal class ApiService
    {
        private const string BaseQueryAddress =
            "https://localhost:7129/";

        private static readonly HttpClient _httpClient = new();

        public static async Task<Rate> GetBanksInfo(string bankName, string currencyName)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"{BaseQueryAddress}api/v1/rates");

            string jsonInfo = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<List<BanksInfo>>(jsonInfo);
            
            return data.FirstOrDefault(bank => bank.Name == bankName).Rates.FirstOrDefault(rate => rate.Name == currencyName);
        }
    }
}
