using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stronger.Application.UseCases.Session;

namespace Stronger.Api.Controllers;

public class SessionController(IMediator mediator) : BaseController(mediator)
{
    [ActionName("Create")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSessionCommand command, CancellationToken cancellationToken)
    {
        return await this.SendAsync(command, cancellationToken);
    }
}
