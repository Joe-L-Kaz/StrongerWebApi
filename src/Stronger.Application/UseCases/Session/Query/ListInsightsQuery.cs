using MediatR;
using Stronger.Application.Responses.Session.Insight;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Session.Query;

public record ListInsightsQuery : IRequest<Response<InsightsResponse>>;
