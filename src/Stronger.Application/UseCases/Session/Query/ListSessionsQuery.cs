using MediatR;
using Stronger.Application.Responses.Session;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Session.Query;

public class ListSessionsQuery : IRequest<Response<List<RetrieveSessionResponse>>>;