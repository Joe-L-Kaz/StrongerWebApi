using System;
using AutoMapper;
using MediatR;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Responses.Exercise;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Exercise;

public class CreateExerciseCommandHandler(
    IRepositoryManager _repo,
    IMapper _mapper
): IRequestHandler<CreateExerciseCommand, Response<CreateExerciseResponse>>
{
    async Task<Response<CreateExerciseResponse>> IRequestHandler<CreateExerciseCommand, Response<CreateExerciseResponse>>.Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return new Response<CreateExerciseResponse>
            {
                StatusCode = 400,
                Error = new Response<CreateExerciseResponse>.ErrorModel
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
            return new Response<CreateExerciseResponse>
            {
                StatusCode = 409,
                Error = new Response<CreateExerciseResponse>.ErrorModel
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
        catch (Exception e)
        {
            return new Response<CreateExerciseResponse>
            {
                StatusCode = 500,
                Error = new Response<CreateExerciseResponse>.ErrorModel
                {
                    StatusCode = 500,
                    Message = e.Message
                }
            };
        }

        return new Response<CreateExerciseResponse>
        {
            StatusCode = 201,
            Content = new CreateExerciseResponse
            {
                Id = exercise.Id
            }
        };
    }
}
