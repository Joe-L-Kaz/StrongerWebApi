using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stronger.Application.Responses.WorkoutPlan;
using Stronger.Application.UseCases.WorkoutPlan.Commands;
using Stronger.Application.UseCases.WorkoutPlan.Queries;
using Stronger.Domain.Responses;

namespace Stronger.Api.Controllers;

public class WorkoutPlanController : BaseController
{
    private readonly IMediator _mediator;
    public WorkoutPlanController(IMediator mediator) : base(mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    [ActionName("Create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateWorkoutPlanCommand cmd, CancellationToken cancellationToken)
    {
        Response<CreateWorkoutPlanResponse> response = await _mediator.Send(cmd, cancellationToken);

        if (response.Content is null)
        {
            return StatusCode(response.StatusCode, response.Error);
        }

        return CreatedAtAction("Retrieve", new { Id = response.Content.Id }, response.Content);
    }

    [HttpGet]
    [ActionName("Retrieve")]
    public async Task<IActionResult> RetrieveAsync([FromQuery] RetrieveWorkoutPlanQuery query, CancellationToken cancellationToken)
    {
        return await this.SendAsync(query, cancellationToken);
    }
}
