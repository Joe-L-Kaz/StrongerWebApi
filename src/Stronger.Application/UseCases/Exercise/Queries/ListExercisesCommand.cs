using MediatR;
using Stronger.Application.Responses.Exercise;

namespace Stronger.Application.UseCases.Exercise.Queries;

public record ListExercisesCommand : IRequest<List<RetrieveExerciseResponse>>;
