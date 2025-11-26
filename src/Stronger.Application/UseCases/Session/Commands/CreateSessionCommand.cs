using MediatR;
using Stronger.Application.Responses.Session;
using Stronger.Domain.Common;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Session;

public record class CreateSessionCommand(
    SessionData SessionData
) : IRequest<Response<CreateSessionResponse>>; 