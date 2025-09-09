using MediatR;
using Stronger.Application.Common.Interfaces;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Responses.User;

namespace Stronger.Application.UseCases.User.Commands;

public class AuthenticateUserCommandHandler(
    IRepositoryManager repository,
    ITokenService tokenService,
    IPasswordService passwordService
) : IRequestHandler<AuthenticateUserCommand, Response<AuthenticateUserResponse>>
{
    async Task<Response<AuthenticateUserResponse>> IRequestHandler<AuthenticateUserCommand, Response<AuthenticateUserResponse>>.Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return new Response<AuthenticateUserResponse>
            {
                StatusCode = 400,
                Error = new Response<AuthenticateUserResponse>.ErrorModel
                {
                    StatusCode = 400,
                    Message = "Request cannot be null"
                }
            };
        }
        UserEntity? user = await repository.Users.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
        {
            return new Response<AuthenticateUserResponse>
            {
                StatusCode = 401,
                Error = new Response<AuthenticateUserResponse>.ErrorModel
                {
                    StatusCode = 401,
                    Message = "Invalid email or password."
                }
            };
        }

        bool validPassword = passwordService.Verify(request.Password, user.PasswordHash);

        if (!validPassword)
        {
            return new Response<AuthenticateUserResponse>
            {
                StatusCode = 401,
                Error = new Response<AuthenticateUserResponse>.ErrorModel
                {
                    StatusCode = 401,
                    Message = "Invalid email or password."
                }
            };
        }

        String accessToken = tokenService.GenerateToken(
            user.Id, user.Forename, user.Surname, user.Dob, user.Email
        );

        return new Response<AuthenticateUserResponse>
        {
            StatusCode = 200,
            Content = new AuthenticateUserResponse
            {
                AccessToken = accessToken
            }
        };
    }
}
