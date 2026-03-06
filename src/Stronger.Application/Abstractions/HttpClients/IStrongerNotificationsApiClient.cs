using System;
using Stronger.Application.Responses.NotificationClient;
using Stronger.Domain.Responses;

namespace Stronger.Application.Abstractions.HttpClients;

public interface IStrongerNotificationsApiClient
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage message, String endpoint, CancellationToken cancellationToken);
}
