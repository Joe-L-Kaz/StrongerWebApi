using MediatR;
using Stronger.Application.Responses.Ping;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Ping.Queries;

public record PingQuery : IRequest<Response<PingResponse>>;
