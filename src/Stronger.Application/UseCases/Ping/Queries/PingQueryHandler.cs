using System;

using MediatR;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Ping.Queries;

public class PingQueryHandler : IRequestHandler<PingQuery, Response>
{
    async Task<Response> IRequestHandler<PingQuery, Response>.Handle(PingQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new Response
        {
            StatusCode = 200,
            Content = new
            {
                Message = "Pong"
            }
        });
    }
}
