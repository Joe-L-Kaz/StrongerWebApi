using System;
using AutoMapper;
using MediatR;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Application.Responses.WorkoutPlan;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.WorkoutPlan.Commands;

public class CreateWorkoutPlanCommandHandler(
    IRepositoryManager repo,
    IMapper mapper,
    IClaimsService claims
) : IRequestHandler<CreateWorkoutPlanCommand, Response<CreateWorkoutPlanResponse>>
{
    async Task<Response<CreateWorkoutPlanResponse>> IRequestHandler<CreateWorkoutPlanCommand, Response<CreateWorkoutPlanResponse>>.Handle(CreateWorkoutPlanCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return new Response<CreateWorkoutPlanResponse>
            {
                StatusCode = 400,
                Error = new Response<CreateWorkoutPlanResponse>.ErrorModel
                {
                    StatusCode = 400,
                    Message = "Request cannot be null"
                }
            };
        }

        if (request.AssociatedExercises.Count == 0)
        {
            return new Response<CreateWorkoutPlanResponse>
            {
                StatusCode = 400,
                Error = new Response<CreateWorkoutPlanResponse>.ErrorModel
                {
                    StatusCode = 400,
                    Message = "Exercise plan must have at least one associated exercise"
                }
            };
        }

        IEnumerable<ExerciseEntity> exercises = await repo.Exercises.ListAsync(null, cancellationToken);
        List<ExerciseEntity> exerciseEntities = exercises.ToList();
        
        if (exerciseEntities.Count == 0)
        {
            return new Response<CreateWorkoutPlanResponse>
            {
                StatusCode = 404,
                Error = new Response<CreateWorkoutPlanResponse>.ErrorModel
                {
                    StatusCode = 404,
                    Message = "There are no exercises in the database"
                }
            };
        }

        foreach (long id in request.AssociatedExercises)
        {
            List<ExerciseEntity> temp = exerciseEntities.Where(e => e.Id == id).ToList();
            if (temp.Count == 0)
            {
                return new Response<CreateWorkoutPlanResponse>
                {
                    StatusCode = 404,
                    Error = new Response<CreateWorkoutPlanResponse>.ErrorModel
                    {
                        StatusCode = 404,
                        Message = "Attempted to add an exercise that does not exist in the database"
                    }
                };
            }
        }

        String userIdString = claims.UserId;
        Guid userId = new Guid(userIdString);

        WorkoutPlanEntity workoutPlan = mapper.Map<WorkoutPlanEntity>(request);
        workoutPlan.CustomerId = userId;
        workoutPlan.Id = await repo.WorkoutPlans.GetNextIdAsync();
        await repo.WorkoutPlans.AddAsync(workoutPlan, cancellationToken);

        foreach (long exerciseId in request.AssociatedExercises)
        {
            WorkoutPlanExerciseEntity temp = new WorkoutPlanExerciseEntity
            {
                ExerciseId = exerciseId,
                WorkoutPlanId = workoutPlan.Id
            };
            await repo.WorkoutPlanExercises.AddAsync(temp, cancellationToken);
        }

        try
        {
            await repo.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            return new Response<CreateWorkoutPlanResponse>
            {
                StatusCode = 500,
                Error = new Response<CreateWorkoutPlanResponse>.ErrorModel
                {
                    StatusCode = 500,
                    Message = e.Message
                }
            };
        }

        return new Response<CreateWorkoutPlanResponse>
        {
            StatusCode = 201,
            Content = new CreateWorkoutPlanResponse
            {
                Id = workoutPlan.Id
            }
        };
    }
}
