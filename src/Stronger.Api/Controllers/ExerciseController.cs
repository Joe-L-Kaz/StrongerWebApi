using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stronger.Application.UseCases.Exercise;
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

        bool someCreated = responses.Any(r => r.StatusCode == 201);

        if (someCreated)
        {
            Response success = new Response
            {
                StatusCode = 201,
                Content = responses.Select(r => r.StatusCode >= 400)
            };

            return StatusCode(success.StatusCode,
                success.Content
            );
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