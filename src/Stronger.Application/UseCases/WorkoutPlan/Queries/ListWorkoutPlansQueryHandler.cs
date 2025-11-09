using System;
using AutoMapper;
using MediatR;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Application.Responses.Exercise;
using Stronger.Application.Responses.WorkoutPlan;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.WorkoutPlan.Queries;

public class ListWorkoutPlansQueryHandler(
    IRepositoryManager repo,
    IMapper mapper,
    IClaimsService claims
) : IRequestHandler<ListWorkoutPlansQuery, Response<IEnumerable<RetrieveWorkoutPlanResponse>>>
{
    async Task<Response<IEnumerable<RetrieveWorkoutPlanResponse>>> IRequestHandler<ListWorkoutPlansQuery, Response<IEnumerable<RetrieveWorkoutPlanResponse>>>.Handle(ListWorkoutPlansQuery request, CancellationToken cancellationToken)
    {
        if (request is null)
        {

            return new Response<IEnumerable<RetrieveWorkoutPlanResponse>>
            {
                StatusCode = 400,
                Error = new Response<IEnumerable<RetrieveWorkoutPlanResponse>>.ErrorModel
                {
                    StatusCode = 400,
                    Message = "Request cannot be null"
                }
            };
        }

        Guid userId = new Guid(claims.UserId);
        List<WorkoutPlanEntity> workouts = (await repo.WorkoutPlans.ListAsync(u => u.CustomerId == userId, cancellationToken)).ToList();

        if (workouts.Count == 0)
        {
            return new Response<IEnumerable<RetrieveWorkoutPlanResponse>>
            {
                StatusCode = 404,
                Error = new Response<IEnumerable<RetrieveWorkoutPlanResponse>>.ErrorModel
                {
                    StatusCode = 404,
                    Message = "Customer does not have any workout plans"
                }
            };
        }

        List<RetrieveWorkoutPlanResponse> workoutPlans = new List<RetrieveWorkoutPlanResponse>();

        foreach (WorkoutPlanEntity plan in workouts)
        {
            RetrieveWorkoutPlanResponse workoutPlan = await BuildPlan(plan, cancellationToken);
            workoutPlans.Add(workoutPlan);
        }

        return new Response<IEnumerable<RetrieveWorkoutPlanResponse>>
        {
            StatusCode = 200,
            Content = workoutPlans
        };
    }
    
    async Task<RetrieveWorkoutPlanResponse> BuildPlan(WorkoutPlanEntity workoutPlan, CancellationToken cancellationToken)
    {
        RetrieveWorkoutPlanResponse workoutPlanResponse = mapper.Map<RetrieveWorkoutPlanResponse>(workoutPlan);

        List<WorkoutPlanExerciseEntity> workoutPlanExercises = (await repo.WorkoutPlanExercises.ListAsync(w => w.WorkoutPlanId == workoutPlan.Id, cancellationToken)).ToList();
        List<RetrieveExerciseResponse> exercises = new();

        foreach (var WorkoutPlanExercise in workoutPlanExercises)
        {
            ExerciseEntity exercise = (await repo.Exercises.GetByIdAsync(WorkoutPlanExercise.ExerciseId, cancellationToken))!;
            RetrieveExerciseResponse exerciseResponse = mapper.Map<RetrieveExerciseResponse>(exercise);
            exercises.Add(exerciseResponse);
        }

        workoutPlanResponse.Exercises = exercises;

        return workoutPlanResponse;
    }
}