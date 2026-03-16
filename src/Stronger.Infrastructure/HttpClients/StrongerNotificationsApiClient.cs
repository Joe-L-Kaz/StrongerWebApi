using Microsoft.Extensions.Configuration;
using Stronger.Application.Abstractions.HttpClients;

namespace Stronger.Infrastructure.HttpClients;

public sealed class StrongerNotificationsApiClient : IStrongerNotificationsApiClient
{
    private readonly HttpClient _client;

    public StrongerNotificationsApiClient(IConfiguration config, HttpClient httpClient)
    {
        _client = httpClient;
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage message, String endpoint, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentException.ThrowIfNullOrEmpty(endpoint);

        if (_client.BaseAddress is null)
            throw new InvalidOperationException("HttpClient.BaseAddress is not configured.");

        message.RequestUri = new Uri(_client.BaseAddress, endpoint);

        return await _client.SendAsync(message, cancellationToken);
    }
}
