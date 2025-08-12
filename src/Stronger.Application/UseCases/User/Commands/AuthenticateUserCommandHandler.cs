using Microsoft.EntityFrameworkCore;
using MediatR;
using Stronger.Application.Common.Interfaces;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.User.Commands;

public class AuthenticateUserCommandHandler(
    IStrongerDbContext context,
    ITokenService tokenService,
    IPasswordService passwordService
) : IRequestHandler<AuthenticateUserCommand, Response>
{
    async Task<Response> IRequestHandler<AuthenticateUserCommand, Response>.Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return new Response
            {
                StatusCode = 400,
                Error = new Response.ErrorModel
                {
                    StatusCode = 400,
                    Message = "Request cannot be null"
                }
            };
        }
        UserEntity? user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user is null)
        {
            return new Response
            {
                StatusCode = 401,
                Error = new Response.ErrorModel
                {
                    StatusCode = 401,
                    Message = "Invalid email or password."
                }
            };
        }

        bool validPassword = passwordService.Verify(request.Password, user.PasswordHash);

        if (!validPassword)
        {
            return new Response
            {
                StatusCode = 401,
                Error = new Response.ErrorModel
                {
                    StatusCode = 401,
                    Message = "Invalid email or password."
                }
            };
        }

        String accessToken = tokenService.GenerateToken(
            user.Id, user.Forename, user.Surname, user.Dob, user.Email
        );

        return new Response
        {
            StatusCode = 200,
            Content = new
            {
                accessToken
            }
        };
    }
}
