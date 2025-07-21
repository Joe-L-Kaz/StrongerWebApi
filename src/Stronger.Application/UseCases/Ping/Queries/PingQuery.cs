using MediatR;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Ping.Queries;

public record PingQuery : IRequest<Response>;
