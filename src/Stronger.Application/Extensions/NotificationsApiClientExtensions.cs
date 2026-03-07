using System.Text;
using System.Text.Json;
using Stronger.Application.Abstractions.HttpClients;
using Stronger.Application.Requests.NotificationsApiClientRequests;
using Stronger.Application.Responses.NotificationClient;

namespace Stronger.Application.Extensions;

public static class NotificationsApiClientExtensions
{
    public static async Task<NotificationClientResponse> RegisterUserDeviceAsync(this IStrongerNotificationsApiClient client, RegisterUserDeviceRequest request, CancellationToken cancellationToken)
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

    public static async Task<NotificationClientResponse> SetTrainingDaysAsync(this IStrongerNotificationsApiClient client, SetTrainingDaysRequest request, CancellationToken cancellationToken)
    {
        dynamic payload = new
        {
            bitMask = request.BitMask
        };
        HttpRequestMessage message = new()
        {
            Method = HttpMethod.Put,
            Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
        };

        var httpResponse = await client.SendAsync(message, $"/api/notification?userId={request.UserId}", cancellationToken);

        NotificationClientResponse response = new()
        {
            StatusCode = (int)httpResponse.StatusCode
        };
        
        return response;
    }
}
