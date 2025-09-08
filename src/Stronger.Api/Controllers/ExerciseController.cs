using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stronger.Application.UseCases.Exercise;
using Stronger.Application.UseCases.Exercise.Queries;
using Stronger.Domain.Responses;

namespace Stronger.Api.Controllers;

public class ExerciseController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateExerciseCommand cmd, CancellationToken cancellationToken)
    {
        return await this.SendAsync(cmd, cancellationToken);
    }

    [HttpPost]
    [ActionName("CreateBulk")]
    [Route("/api/[Controller]/[Action]")]
    public async Task<IActionResult> CreateBulkAsync([FromBody] List<CreateExerciseCommand> cmds, CancellationToken cancellationToken)
    {
        if (cmds is null || cmds.Count == 0)
        {
            Response response = new Response
            {
                StatusCode = 400,
                Error = new Response.ErrorModel
                {
                    StatusCode = 400,
                    Message = "Request cannot be null"
                }
            };

            return StatusCode(400, response);
        }

        List<Response> responses = new List<Response>();

        foreach (var cmd in cmds)
        {
            Response response = await mediator.Send(cmd, cancellationToken);
            responses.Add(response);
        }

        int total = cmds.Count;
        int succeeded = responses.Count(r => r.StatusCode >= 200 && r.StatusCode < 300);
        int failed = total - succeeded;

        if (succeeded > 0)
        {
            var failedItems = cmds
                .Select((cmd, index) => new { cmd, index })
                .Zip(responses, (c, r) => new
                {
                    c.index,
                    name = c.cmd.Name,
                    status = r.StatusCode,
                    error = r.Error?.Message
                })
                .Where(x => x.status >= 400)
                .ToList();

            var content = new
            {
                summary = new { total, succeeded, failed },
                message = failed > 0 ? $"{failed} failed to save" : "All items created",
                failedItems
            };

            var statusCode = succeeded == total ? StatusCodes.Status201Created : StatusCodes.Status200OK;
            return StatusCode(statusCode, content);
        }

        Response fail = new Response
        {
            StatusCode = 400,
            Error = new Response.ErrorModel
            {
                StatusCode = 400,
                Message = "None of the exercises could be created"
            }
        };

        return StatusCode(fail.StatusCode, fail.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] long id, CancellationToken cancellationToken)
    {
        return await this.SendAsync(new RetrieveExerciseCommand(id), cancellationToken);
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