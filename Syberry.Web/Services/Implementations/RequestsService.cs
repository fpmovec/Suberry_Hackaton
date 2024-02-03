using Microsoft.Extensions.Options;
using Syberry.Web.Models;
using Syberry.Web.Services.Abstractions;

namespace Syberry.Web.Services.Implementations;

public class RequestsService(HttpClient client, IOptions<AppSettings> options) : IRequestsService
{
    private readonly AppSettings _settings = options.Value;
    public Task<Bank> GetAsync(string url, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}