using System;
using System.Security.Claims;
using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Application.Responses.Session;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Session.Commands;

public class CreateSessionCommandHandler(
    IRepositoryManager repository,
    IMapper mapper,
    IClaimsService claims
) : IRequestHandler<CreateSessionCommand, Response<CreateSessionResponse>>
{
    async Task<Response<CreateSessionResponse>> IRequestHandler<CreateSessionCommand, Response<CreateSessionResponse>>.Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        if(request is null)
        {
            return new Response<CreateSessionResponse>
            {
                StatusCode = 400,
                Error = new Response<CreateSessionResponse>.ErrorModel
                {
                    StatusCode = 400,
                    Message = "Request cannot be null" 
                }
            };
        }

        if(request.SessionData is null)
        {
            return new Response<CreateSessionResponse>
            {
                StatusCode = 400,
                Error = new Response<CreateSessionResponse>.ErrorModel
                {
                    StatusCode = 400,
                    Message = "Session must contain session data" 
                }
            };
        }

        if(request.SessionData.Exercises.Count < 1)
        {
            return new Response<CreateSessionResponse>
            {
                StatusCode = 400,
                Error = new Response<CreateSessionResponse>.ErrorModel
                {
                    StatusCode = 400,
                    Message = "Session Must have Exercises" 
                }
            };
        }

        SessionEntity session = mapper.Map<SessionEntity>(request);
        session.CompletedAt = DateOnly.FromDateTime(DateTime.Now);
        
        string userId = claims.UserId;
        session.UserId = new Guid(userId);

        await repository.Sessions.AddAsync(session, cancellationToken);

        try
        {
            await repository.SaveChangesAsync(cancellationToken);
        }
        catch(Exception e)
        {
            return new Response<CreateSessionResponse>
            {
                StatusCode = 500,
                Error = new Response<CreateSessionResponse>.ErrorModel
                {
                    StatusCode = 500,
                    Message = e.Message
                }
            };
        }

        return new Response<CreateSessionResponse>
        {
            StatusCode = 201,
            Content = new CreateSessionResponse
            {
                Id = session.Id
            }
        };
    }
}
