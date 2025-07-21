using AutoMapper;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.EntityFrameworkCore;
using Stronger.Application.Common.Interfaces;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.User.Commands;

public class CreateNewUserCommandHandler(IStrongerDbContext context, IMapper mapper) : IRequestHandler<CreateNewUserCommand, Response>
{
    async Task<Response> IRequestHandler<CreateNewUserCommand, Response>.Handle(CreateNewUserCommand request, CancellationToken cancellationToken)
    {
#warning Dont forget to scramble details later
        UserEntity entity = mapper.Map<UserEntity>(request);

        UserEntity? temp = await context.Users.FirstOrDefaultAsync(u => u.Email == entity.Email, cancellationToken);

        if (temp is not null)
        {
            return new Response
            {
                StatusCode = 409,
                Content = new
                {
                    Message = "Email already in use."
                }
            };
        } 
        

        await context.Users.AddAsync(entity);
        await context.SaveChangesAsync(cancellationToken);

        return new Response
        {
            StatusCode = 201,
            Content = new
            {
                Id = entity.Id
            }
        };
    }
}
