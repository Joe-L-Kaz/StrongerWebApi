using MediatR;
using Stronger.Application.Responses.User;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.User.Commands;

public record SetTrainingDaysCommand(short BitMask) : IRequest<Response<SetTrainingDaysResponse>>;