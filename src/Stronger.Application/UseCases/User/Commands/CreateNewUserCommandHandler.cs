using AutoMapper;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.EntityFrameworkCore;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.User.Commands;

public class CreateNewUserCommandHandler(
    IRepositoryManager repository,
    IMapper mapper,
    IPasswordService passwordService
) : IRequestHandler<CreateNewUserCommand, Response>
{
    async Task<Response> IRequestHandler<CreateNewUserCommand, Response>.Handle(CreateNewUserCommand request, CancellationToken cancellationToken)
    {
#warning Dont forget to scramble details later
        UserEntity entity = mapper.Map<UserEntity>(request);

        UserEntity? temp = await repository.Users.GetByEmailAsync(entity.Email,cancellationToken);
        if (temp is not null)
        {
            return new Response
            {
                StatusCode = 409,
                Error = new Response.ErrorModel
                {
                    StatusCode = 409,
                    Message = "Email already in use"
                }
            };
        }

        string password = request.Password;
        if (!passwordService.Validate(password))
        {
            return new Response
            {
                StatusCode = 400,
                Error = new Response.ErrorModel
                {
                    StatusCode = 400,
                    Message = "Password does not meet security requirements"
                }
            };
        }

        entity.PasswordHash = passwordService.Hash(password);


        await repository.Users.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new Response
        {
            StatusCode = 201,
            Content = new
            {
                entity.Id
            }
        };
    }
}
