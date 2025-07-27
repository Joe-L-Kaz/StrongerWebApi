using MediatR;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.User.Commands;

public record AuthenticateUserCommand(
    String Email,
    String Password
) : IRequest<Response>;