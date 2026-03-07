using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Stronger.Application.Responses.Exercise;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Exercise.Commands;

public record CreateBulkExerciseCsvCommand(IFormFile File): IRequest<Response<CreateExerciseResponse>>;