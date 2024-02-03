using Syberry.Web.Models;
using Syberry.Web.Services.Abstractions;

namespace Syberry.Web.Services.Implementations;

public class RequestsService(HttpClient client) : IRequestsService
{
    public Task PostAsync(string url, Bank data, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Bank> GetAsync(string url, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}