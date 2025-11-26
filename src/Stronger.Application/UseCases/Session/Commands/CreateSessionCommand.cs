using MediatR;
using Stronger.Domain.Common;

namespace Stronger.Application.UseCases.Session;

public record class CreateSessionCommand(
    SessionData SessionData
) : IRequest<CreateSessionResponse>; 