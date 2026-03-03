using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stronger.Application.UseCases.Session;
using Stronger.Application.UseCases.Session.Query;

namespace Stronger.Api.Controllers;

public class SessionController(IMediator mediator) : BaseController(mediator)
{
    [ActionName("Create")]
    [HttpPost]
    public Task<IActionResult> CreateAsync([FromBody] CreateSessionCommand command, CancellationToken cancellationToken)
    {
        return this.SendAsync(command, cancellationToken);
    }

    [ActionName("List")]
    [HttpGet]
    public Task<IActionResult> ListAsync(CancellationToken cancellationToken)
    {
        ListSessionsQuery query = new();
        return this.SendAsync(query, cancellationToken);
    }

    [ActionName("Insights")]
    [HttpGet]
    [Route("/api/[controller]/[action]")]
    public Task<IActionResult> InsightsAsync(CancellationToken cancellationToken)
    {
        return this.SendAsync(new ListInsightsQuery(), cancellationToken);
    }
}
