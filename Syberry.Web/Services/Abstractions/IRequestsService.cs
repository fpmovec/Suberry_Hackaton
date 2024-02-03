using Syberry.Web.Models;

namespace Syberry.Web.Services.Abstractions;

public interface IRequestsService
{
    Task<Bank> GetAsync(string url, CancellationToken cancellationToken = default);
}