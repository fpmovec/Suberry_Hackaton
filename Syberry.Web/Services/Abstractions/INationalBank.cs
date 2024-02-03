using Syberry.Web.Models;

namespace Syberry.Web.Services.Abstractions;

public interface INationalBankService
{
    Task<Bank> GetNationalBankAsync();
    Task<CurrencyView> GetCurrencies();
}