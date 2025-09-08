using MediatR;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Exercise.Queries;

public record RetrieveExerciseCommand(
    long Id
) : IRequest<Response>;
