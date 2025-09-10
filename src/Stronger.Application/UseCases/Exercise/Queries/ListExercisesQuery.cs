using MediatR;
using Stronger.Application.Responses.Exercise;
using Stronger.Domain.Enums;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Exercise.Queries;

public record ListExercisesQuery(
    String? Name,
    MuscleGroup? PrimaryMuscleGroup,
    MuscleGroup? SecondaryMuscleGroup,
    ExerciseType? ExerciseType,
    ForceType? ForceType
) : IRequest<Response<IEnumerable<RetrieveExerciseResponse>>>;
