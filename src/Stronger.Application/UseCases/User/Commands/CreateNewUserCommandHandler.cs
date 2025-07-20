using MediatR;
using Stronger.Application.Common.Interfaces;
using Stronger.Domain.Entities;

namespace Stronger.Application.UseCases.User.Commands;

public class CreateNewUserCommandHandler(IStrongerDbContext context) : IRequestHandler<CreateNewUserCommand, Guid>
{
    async Task<Guid> IRequestHandler<CreateNewUserCommand, Guid>.Handle(CreateNewUserCommand request, CancellationToken cancellationToken)
    {
        #warning Dont forget to scramble details later
        UserEntity entity = new UserEntity
        {
            Forename = request.Forename,
            Surname = request.Surname,
            Dob = request.Dob,
            Email = request.Email,
            PasswordHash = request.Password
        };
        await context.Users.AddAsync(entity);
        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
