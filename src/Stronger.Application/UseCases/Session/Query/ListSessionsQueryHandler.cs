using System;
using AutoMapper;
using MediatR;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Application.Responses.Session;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Session.Query;

public class ListSessionsQueryHandler(
    IRepositoryManager repo,
    IMapper mapper,
    IClaimsService claims
) : IRequestHandler<ListSessionsQuery, Response<List<RetrieveSessionResponse>>>
{
    async Task<Response<List<RetrieveSessionResponse>>> IRequestHandler<ListSessionsQuery, Response<List<RetrieveSessionResponse>>>.Handle(ListSessionsQuery request, CancellationToken cancellationToken)
    {
        if(request is null)
        {
            return new Response<List<RetrieveSessionResponse>>
            {
                StatusCode = 400,
                Error = new Response<List<RetrieveSessionResponse>>.ErrorModel
                {
                    StatusCode = 400,
                    Message = "Request cannot be null" 
                }
            };
        }
        String idStr = claims.UserId;
        Guid userId = new Guid(idStr);
        IEnumerable<SessionEntity> sessionEntities = await repo.Sessions.ListAsync(s => s.UserId == userId, cancellationToken);

        List<RetrieveSessionResponse> sessionResponses = new();

        foreach (SessionEntity sessionEntity in sessionEntities)
        {
            RetrieveSessionResponse sessionResponse = mapper.Map<RetrieveSessionResponse>(sessionEntity);
            sessionResponses.Add(sessionResponse);   
        }

        return new Response<List<RetrieveSessionResponse>>
        {
            StatusCode = 200,
            Content = sessionResponses
        };
    
    }   
}
