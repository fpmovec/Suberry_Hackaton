using Syberry.Web.Models;

namespace Syberry.Web.Services.Abstractions;

public interface IRequestsService
{
    Task PostAsync(string url, Bank data, CancellationToken cancellationToken = default);
    Task<Bank> GetAsync(string url, CancellationToken cancellationToken = default);
}