using System;

namespace Stronger.Application.Requests.NotificationsApiClientRequests;

public record SetTrainingDaysRequest(Guid UserId, short BitMask);