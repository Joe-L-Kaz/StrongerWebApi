using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stronger.Application.UseCases.Exercise;

namespace Stronger.Api.Controllers;

public class ExerciseController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateExerciseCommand cmd, CancellationToken cancellationToken)
    {
        return await this.SendAsync(cmd, cancellationToken);
    }

    [HttpPost]
    [ActionName("CreateBulk")]
    [Route("/api/[Controller]/[Action]")]
    public async Task<IActionResult> CreateBulkAsync()
    {
        return null!;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return null!;
    }

    [HttpGet]
    [ActionName("List")]
    [Route("/api/[Controller]/[Action]")]
    public async Task<IActionResult> ListAsync()
    {
        string str = "ww";
        return null!;
    }
}