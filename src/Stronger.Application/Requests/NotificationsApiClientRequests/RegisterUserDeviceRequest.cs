using System;

namespace Stronger.Application.Requests.NotificationsApiClientRequests;

public record RegisterUserDeviceRequest(
    Guid UserId,
    String DeviceToken,
    String DeviceType,
    byte TrainingDays,
    String FirstName
);
