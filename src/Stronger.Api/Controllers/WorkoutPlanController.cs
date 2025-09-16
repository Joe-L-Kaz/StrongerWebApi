using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stronger.Application.UseCases.WorkoutPlan.Commands;

namespace Stronger.Api.Controllers;

public class WorkoutPlanController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    [ActionName("Create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateWorkoutPlanCommand cmd, CancellationToken cancellationToken)
    {
        return await this.SendAsync(cmd, cancellationToken);
    }

}
