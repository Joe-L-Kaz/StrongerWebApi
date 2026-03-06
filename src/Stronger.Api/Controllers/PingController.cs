using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MediatR;


using Stronger.Application.UseCases.Ping.Queries;

namespace Stronger.Api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class PingController(IMediator mediator) : BaseController(mediator)
    {
        [HttpGet]
        public Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            return this.SendAsync(new PingQuery(), cancellationToken);
        }
    }
}
