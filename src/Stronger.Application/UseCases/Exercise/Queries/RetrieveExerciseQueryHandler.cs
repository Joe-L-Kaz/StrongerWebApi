using System;
using MediatR;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Responses.Exercise;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Exercise.Queries;

public class RetrieveExerciseCommandHandler(
    IRepositoryManager _repo
) : IRequestHandler<RetrieveExerciseQuery, Response<RetrieveExerciseResponse>>
{
    async Task<Response<RetrieveExerciseResponse>> IRequestHandler<RetrieveExerciseQuery, Response<RetrieveExerciseResponse>>.Handle(RetrieveExerciseQuery request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return new Response<RetrieveExerciseResponse>
            {
                StatusCode = 400,
                Error = new Response<RetrieveExerciseResponse>.ErrorModel
                {
                    StatusCode = 400,
                    Message = "Request cannot be null."
                }
            };
        }

        ExerciseEntity? exercise = await _repo.Exercises.GetByIdAsync(request.Id, cancellationToken);

        if (exercise is null)
        {
            return new Response<RetrieveExerciseResponse>
            {
                StatusCode = 404,
                Error = new Response<RetrieveExerciseResponse>.ErrorModel
                {
                    Message = $"Exercise with Id: {request.Id} not found"
                }
            };
        }

        return new Response<RetrieveExerciseResponse>
        {
            StatusCode = 200,
            Content = new RetrieveExerciseResponse
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
                ImagePath = exercise.ImagePath,
                PrimaryMuscleGroup = exercise.PrimaryMuscleGroup,
                SecondaryMuscleGroup = exercise.SecondaryMuscleGroup,
                ExerciseType = exercise.ExerciseType,
                ForceType = exercise.ForceType
            }
        };

    }
}
