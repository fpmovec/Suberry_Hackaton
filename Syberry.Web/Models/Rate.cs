using Newtonsoft.Json;

namespace Syberry.Web.Models;

public class Rate
{
    public string Name { get; set; }
    
    public double SellRate { get; set; }
    
    public double BuyRate { get; set; }
    
    public DateTime KursDateTime { get; set; }
}