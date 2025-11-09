using MediatR;
using Stronger.Application.Responses.WorkoutPlan;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.WorkoutPlan.Commands;

public record CreateWorkoutPlanCommand(
    String Name,
    List<long> AssociatedExercises
): IRequest<Response<CreateWorkoutPlanResponse>>;

