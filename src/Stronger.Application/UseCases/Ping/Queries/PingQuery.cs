using MediatR;

namespace Stronger.Application.UseCases.Ping.Queries;

public record PingQuery : IRequest<String>;
