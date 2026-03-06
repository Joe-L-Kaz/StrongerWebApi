using System.Text;
using System.Text.Json;
using Stronger.Application.Abstractions.HttpClients;
using Stronger.Application.Requests.NotificationsApiClientRequests;
using Stronger.Application.Responses.NotificationClient;

namespace Stronger.Application.Extensions;

public static class NotificationsApiClientExtensions
{
    public static async Task<NotificationClientResponse> RegisterUserDevice(this IStrongerNotificationsApiClient client, RegisterUserDeviceRequest request, CancellationToken cancellationToken)
    {
        HttpRequestMessage message = new()
        {
            Method = HttpMethod.Post,
            Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
        };

        var httpResponse = await client.SendAsync(message, "/api/notification", cancellationToken);

        NotificationClientResponse response = new()
        {
            StatusCode = (int)httpResponse.StatusCode
        };
        
        return response;
    }
}
