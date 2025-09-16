using System;
using AutoMapper;
using MediatR;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Responses.Exercise;
using Stronger.Application.Responses.WorkoutPlan;
using Stronger.Application.UseCases.Exercise.Queries;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.WorkoutPlan.Queries;

public class RetrieveWorkoutPlanQueryHandler(
    IRepositoryManager repo,
    IMapper mapper
) : IRequestHandler<RetrieveWorkoutPlanQuery, Response<RetrieveWorkoutPlanResponse>>
{
    async Task<Response<RetrieveWorkoutPlanResponse>> IRequestHandler<RetrieveWorkoutPlanQuery, Response<RetrieveWorkoutPlanResponse>>.Handle(RetrieveWorkoutPlanQuery request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return new Response<RetrieveWorkoutPlanResponse>
            {
                StatusCode = 400,
                Error = new Response<RetrieveWorkoutPlanResponse>.ErrorModel
                {
                    StatusCode = 400,
                    Message = "Request cannot be null"
                }
            };
        }

        WorkoutPlanEntity? workoutPlan = await repo.WorkoutPlans.GetByIdAsync(request.Id, cancellationToken);

        if (workoutPlan is null)
        {
            return new Response<RetrieveWorkoutPlanResponse>
            {
                StatusCode = 404,
                Error = new Response<RetrieveWorkoutPlanResponse>.ErrorModel
                {
                    StatusCode = 404,
                    Message = "Workout dlan does not exists"
                }
            };
        }

        IEnumerable<WorkoutPlanExerciseEntity> workoutPlanExerciseEntities = await repo.WorkoutPlanExercises.ListAsync(e => e.WorkoutPlanId == workoutPlan.Id, cancellationToken);
        List<WorkoutPlanExerciseEntity> workoutPlanExercises = workoutPlanExerciseEntities.ToList();
        if (workoutPlanExercises.Count == 0)
        {
            return new Response<RetrieveWorkoutPlanResponse>
            {
                StatusCode = 404,
                Error = new Response<RetrieveWorkoutPlanResponse>.ErrorModel
                {
                    StatusCode = 404,
                    Message = "There are no exercise plans associated with this plan"
                }
            };
        }

        IEnumerable<ExerciseEntity> exerciseEntities = await repo.Exercises.ListAsync(null, cancellationToken);
        List<ExerciseEntity> exercises = exerciseEntities.ToList();
        if (exercises.Count == 0)
        {
            return new Response<RetrieveWorkoutPlanResponse>
            {
                StatusCode = 404,
                Error = new Response<RetrieveWorkoutPlanResponse>.ErrorModel
                {
                    StatusCode = 404,
                    Message = "There are no exercises in the database"
                }
            };
        }
        List<RetrieveExerciseResponse> exerciseResponses = new List<RetrieveExerciseResponse>();
        foreach (var workoutPlanExerciseEntity in workoutPlanExerciseEntities)
        {
            ExerciseEntity? exercise = exercises.FirstOrDefault(e => e.Id == workoutPlanExerciseEntity.ExerciseId);

            if (exercise is null)
            {
                return new Response<RetrieveWorkoutPlanResponse>
                {
                    StatusCode = 404,
                    Error = new Response<RetrieveWorkoutPlanResponse>.ErrorModel
                    {
                        StatusCode = 404,
                        Message = $"Exercise with {workoutPlanExerciseEntity.ExerciseId} does not exist"
                    }
                };
            }

            RetrieveExerciseResponse exerciseResponse = mapper.Map<RetrieveExerciseResponse>(exercise);
            exerciseResponses.Add(exerciseResponse);
        }

        RetrieveWorkoutPlanResponse response = mapper.Map<RetrieveWorkoutPlanResponse>(workoutPlan);
        response.Exercises = exerciseResponses;
        return new Response<RetrieveWorkoutPlanResponse>
        {
            StatusCode = 200,
            Content = response
        };
        
    }
}
