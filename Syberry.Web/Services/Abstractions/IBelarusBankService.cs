using Syberry.Web.Models;

namespace Syberry.Web.Services.Abstractions;

public interface IBelarusBankService
{
    Task<IEnumerable<Rate>> GetBelarusBankRatesAsync();
}