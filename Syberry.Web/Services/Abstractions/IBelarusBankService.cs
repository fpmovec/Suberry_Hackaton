using Syberry.Web.Models;

namespace Syberry.Web.Services.Abstractions;

public interface IBelarusBankService
{
    Task<List<Rate>> BelarusBankRates();
}