using MediatR;

namespace Stronger.Application.UseCases.User.Commands;

public record class CreateNewUserCommand(
    String Forename,
    String Surname,
    DateOnly Dob,
    String Email,
    String Password
) : IRequest<Guid>;
