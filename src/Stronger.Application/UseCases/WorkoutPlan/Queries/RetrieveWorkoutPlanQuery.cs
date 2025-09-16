using System;
using MediatR;
using Stronger.Application.Responses.WorkoutPlan;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.WorkoutPlan.Queries;

public record RetrieveWorkoutPlanQuery(
    long Id
) : IRequest<Response<RetrieveWorkoutPlanResponse>>;

