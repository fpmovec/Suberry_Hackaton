using Newtonsoft.Json;

namespace Syberry.Web.Models;

public class Rate
{
    [JsonProperty("kurs_date_time")]
    public DateTime KursDateTime { get; set; }
    
    [JsonProperty("USDCARD_in")]
    public double UsdIn { get; set; }
    [JsonProperty("USDCARD_out")]
    public double UsdOut { get; set; }
    
    [JsonProperty("EURCARD_in")]
    public double EuroIn { get; set; }
    [JsonProperty("EURCARD_out")]
    public double EuroOut { get; set; }
    
    [JsonProperty("RUBCARD_in")]
    public double RubIn { get; set; }
    [JsonProperty("RUBCARD_out")]
    public double RubOut { get; set; }
}