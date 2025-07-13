using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MediatR;


using Stronger.Application.UseCases.Ping.Queries;

namespace Stronger.Api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class PingController(IMediator mediator) : ControllerBase
    {
        public async Task<IActionResult> Get()
        {
            string response = await mediator.Send(new PingQuery());
            return Ok(response);
        }
    }
}
