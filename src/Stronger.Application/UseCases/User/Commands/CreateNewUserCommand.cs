using MediatR;
using Stronger.Application.Responses.User;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.User.Commands;

public record class CreateNewUserCommand(
    String Forename,
    String Surname,
    DateOnly Dob,
    String Email,
    String Password
) : IRequest<Response<CreateNewUserResponse>>;
