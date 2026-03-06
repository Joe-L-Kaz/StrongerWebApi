using Microsoft.Extensions.Configuration;
using Stronger.Application.Abstractions.HttpClients;

namespace Stronger.Infrastructure.HttpClients;

public sealed class StrongerNotificationsApiClient : IStrongerNotificationsApiClient
{
    private readonly HttpClient _client;

    public StrongerNotificationsApiClient(IConfiguration config, HttpClient httpClient)
    {
        // String baseAddress = config["NotificationsApiConfiguration:NotificationsApiBaseAddress"] ?? throw new ArgumentException("NotificationsApiBaseAddress configuration is required.", nameof(config));
        // String apiKey = config["NotificationsApiConfiguration:NotificationsApiKey"] ?? throw new ArgumentException("NotificationsApiKey configuration is required.", nameof(config));
        // String apiKeyHeaderName = config["NotificationsApiConfiguration:ApiKeyHeaderName"] ?? throw new ArgumentException("ApiKeyHeaderName configuration is required.", nameof(config));

        // httpClient.BaseAddress = new Uri(baseAddress);
        // httpClient.DefaultRequestHeaders.Add(apiKeyHeaderName, apiKey);

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
