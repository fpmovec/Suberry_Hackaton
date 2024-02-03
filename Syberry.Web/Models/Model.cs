namespace Syberry.Web.Models;

public class Rate
{
    public DateTime KursDateTime { get; set; }
    
    public double UsdIn { get; set; }
    public double UsdOut { get; set; }
    
    public double EuroIn { get; set; }
    public double EuroOut { get; set; }
    
    public double RubIn { get; set; }
    public double RubOut { get; set; }
}