namespace Syberry.Web.Models.Dto;

public class AlpfabankRateDto
{
    public double SellRate { get; set; }
    
    public string SellIso { get; set; }
    
    public double BuyRate { get; set; }
    
    public string BuyIso { get; set; }
    
    public DateTime Date { get; set; }
}