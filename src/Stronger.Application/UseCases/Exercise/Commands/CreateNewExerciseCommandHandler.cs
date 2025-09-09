using System;
using AutoMapper;
using MediatR;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Exercise;

public class CreateNewExerciseCommandHandler(
    IRepositoryManager _repo,
    IMapper _mapper
): IRequestHandler<CreateExerciseCommand, Response>
{
    async Task<Response> IRequestHandler<CreateExerciseCommand, Response>.Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return new Response
            {
                StatusCode = 400,
                Error = new Response.ErrorModel
                {
                    StatusCode = 400,
                    Message = "Request cannot be null"
                }
            };
        }

        String nameNormalized = request.Name.Trim().ToUpper();

        var exists = await _repo.Exercises.AnyAsync(
            nameNormalized,
            cancellationToken
        );

        if (exists)
        {
            return new Response
            {
                StatusCode = 409,
                Error = new Response.ErrorModel
                {
                    StatusCode = 409,
                    Message = $"{request.Name} already exists."
                }
            };
        }

        ExerciseEntity exercise = _mapper.Map<ExerciseEntity>(request);
        exercise.NameNormalized = nameNormalized;

        await _repo.Exercises.AddAsync(exercise, cancellationToken);

        try
        {
            await _repo.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return new Response
            {
                StatusCode = 400,
                Error = new Response.ErrorModel
                {
                    StatusCode = 400,
                    Message = "Could not save exercise try again later."
                }
            };
        }

        return new Response
        {
            StatusCode = 201,
            Content = new
            {
                exercise.Id
            }
        };
    }
}
