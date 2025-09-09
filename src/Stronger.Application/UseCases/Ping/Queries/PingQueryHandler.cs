using MediatR;
using Stronger.Application.Responses.Ping;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Ping.Queries;

public class PingQueryHandler : IRequestHandler<PingQuery, Response<PingResponse>>
{
    async Task<Response<PingResponse>> IRequestHandler<PingQuery, Response<PingResponse>>.Handle(PingQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new Response<PingResponse>
        {
            StatusCode = 200,
            Content = new PingResponse
            {
                Message = "Pong"
            }
        });
    }
}
