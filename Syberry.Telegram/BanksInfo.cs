using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syberry.Telegram
{
    public class BanksInfo
    {
        [JsonProperty("name")]
        public string? Name;

        [JsonProperty("rates")]
        public List<Rate>? Rates;
    }

    public class Rate
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("sellRate")]
        public decimal SellRate;

        [JsonProperty("buyRate")]
        public decimal BuyRate;
       
        [JsonProperty("kursDateTime")]
        public DateTime? Time;
    }
}
