namespace Syberry.Web.Models.Dto;

public class StatisticsDto
{
    public string ChartImage { get; set; }
    public double MinRate { get; set; }
    public double MaxRate { get; set; }
    public double RateAtThePeriodStart { get; set; }
    public double RateAtThePeriodend { get; set; }
    public double AverageRate { get; set; }
}