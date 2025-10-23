using MediatR;
using Stronger.Application.Responses.WorkoutPlan;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.WorkoutPlan.Queries;

public record ListWorkoutPlansQuery : IRequest<Response<IEnumerable<RetrieveWorkoutPlanResponse>>>;