using MediatR;
using Stronger.Application.Responses.Exercise;
using Stronger.Domain.Enums;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Exercise;

public record CreateExerciseCommand(
    String Name,
    String Description,
    String? ImagePath,
    MuscleGroup PrimaryMuscleGroup,
    MuscleGroup? SecondaryMuscleGroup,
    ExerciseType ExerciseType,
    ForceType ForceType
) : IRequest<Response<CreateExerciseResponse>>;
