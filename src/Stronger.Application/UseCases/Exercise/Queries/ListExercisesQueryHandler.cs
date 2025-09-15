using AutoMapper;
using MediatR;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Responses.Exercise;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Exercise.Queries;

public class ListExercisesQueryHandle(
    IRepositoryManager repo,
    IMapper mapper
) : IRequestHandler<ListExercisesQuery, Response<IEnumerable<RetrieveExerciseResponse>>>
{
    async Task<Response<IEnumerable<RetrieveExerciseResponse>>> IRequestHandler<ListExercisesQuery, Response<IEnumerable<RetrieveExerciseResponse>>>.Handle(ListExercisesQuery request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return new Response<IEnumerable<RetrieveExerciseResponse>>
            {
                StatusCode = 400,
                Error = new Response<IEnumerable<RetrieveExerciseResponse>>.ErrorModel
                {
                    StatusCode = 400,
                    Message = "request cannot be null"
                }
            };
        }

        IEnumerable<ExerciseEntity> exercises = await repo.Exercises.ListAsync(null, cancellationToken);

        if (!String.IsNullOrWhiteSpace(request.Name))
            exercises = exercises.Where(e => e.Name == request.Name);

        if (request.PrimaryMuscleGroup is not null)
            exercises = exercises.Where(e => e.PrimaryMuscleGroup == request.PrimaryMuscleGroup);

        if (request.SecondaryMuscleGroup is not null)
            exercises = exercises.Where(e => e.SecondaryMuscleGroup == request.SecondaryMuscleGroup);

        if (request.ExerciseType is not null)
            exercises = exercises.Where(e => e.ExerciseType == request.ExerciseType);

        if (request.ForceType is not null)
            exercises = exercises.Where(e => e.ForceType == request.ForceType);

        List<RetrieveExerciseResponse> responses = new List<RetrieveExerciseResponse>();

        foreach (var exercise in exercises)
        {
            RetrieveExerciseResponse response = mapper.Map<RetrieveExerciseResponse>(exercise);
            responses.Add(response);  
        }

        return new Response<IEnumerable<RetrieveExerciseResponse>>
        {
            StatusCode = 200,
            Content = responses.AsEnumerable()
        };
    }
}
