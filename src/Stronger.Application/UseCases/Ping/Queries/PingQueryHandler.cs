using System;

using MediatR;

namespace Stronger.Application.UseCases.Ping.Queries;

public class PingQueryHandler : IRequestHandler<PingQuery, String>
{
    async Task<String> IRequestHandler<PingQuery, String>.Handle(PingQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult("Pong");
    }
}
