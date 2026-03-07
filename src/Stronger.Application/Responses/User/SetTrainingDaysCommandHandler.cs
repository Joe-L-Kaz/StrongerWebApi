using System;
using MediatR;
using Stronger.Application.Abstractions.HttpClients;
using Stronger.Application.Common.Interfaces;
using Stronger.Application.Extensions;
using Stronger.Application.Requests.NotificationsApiClientRequests;
using Stronger.Application.Responses.NotificationClient;
using Stronger.Application.UseCases.User.Commands;
using Stronger.Domain.Responses;

namespace Stronger.Application.Responses.User;

public class SetTrainingDaysCommandHandler(IStrongerNotificationsApiClient notificationsApiClient, IClaimsService claims) : IRequestHandler<SetTrainingDaysCommand, Response<SetTrainingDaysResponse>>
{
    async Task<Response<SetTrainingDaysResponse>> IRequestHandler<SetTrainingDaysCommand, Response<SetTrainingDaysResponse>>.Handle(SetTrainingDaysCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        Guid userId = new(claims.UserId);
        SetTrainingDaysRequest trainingDaysRequest = new(userId, request.BitMask);
        NotificationClientResponse repsonse = await notificationsApiClient.SetTrainingDaysAsync(trainingDaysRequest, cancellationToken);
        return new Response<SetTrainingDaysResponse>
        {
            StatusCode = repsonse.StatusCode
        };
    }
}
