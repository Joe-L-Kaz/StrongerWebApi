using MediatR;
using Stronger.Application.Responses.User;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.User.Commands;

public record AuthenticateUserCommand(
    String Email,
    String Password
) : IRequest<Response<AuthenticateUserResponse>>;