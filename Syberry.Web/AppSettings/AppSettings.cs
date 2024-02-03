using Syberry.Web.Services.Implementations;

namespace Syberry.Web;

public class AppSettings
{
    public const string SectionName = nameof(AppSettings);
    public BelarusBankSettings BelarusBankSettings { get; set; }
    public BankRedisKeys BankRedisKeys { get; set; }
    public AlphaBankSettings AlphaBankSettings { get; set; }
    public NationalBankSettings NationalBankSettings { get; set; }

}