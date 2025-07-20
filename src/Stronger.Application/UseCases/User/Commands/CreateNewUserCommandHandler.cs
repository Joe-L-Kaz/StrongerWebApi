using MediatR;

namespace Stronger.Application.UseCases.User.Commands;

public class CreateNewUserCommandHandler : IRequestHandler<CreateNewUserCommand, Guid>
{
    Task<Guid> IRequestHandler<CreateNewUserCommand, Guid>.Handle(CreateNewUserCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Guid.NewGuid());
    }
}
