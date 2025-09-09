using System;
using MediatR;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Exercise.Queries;

public class RetrieveExerciseCommandHandler(
    IRepositoryManager _repo
) : IRequestHandler<RetrieveExerciseCommand, Response>
{
    async Task<Response> IRequestHandler<RetrieveExerciseCommand, Response>.Handle(RetrieveExerciseCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return new Response
            {
                StatusCode = 400,
                Error = new Response.ErrorModel
                {
                    StatusCode = 400,
                    Message = "Request cannot be null."
                }
            };
        }

        ExerciseEntity? exercise = await _repo.Exercises.GetByIdAsync(request.Id, cancellationToken);

        if (exercise is null)
        {
            return new Response
            {
                StatusCode = 404,
                Error = new Response.ErrorModel
                {
                    Message = $"Exercise with Id: {request.Id} not found"
                }
            };
        }

        return new Response
        {
            StatusCode = 200,
            Content = exercise
        };

    }
}
