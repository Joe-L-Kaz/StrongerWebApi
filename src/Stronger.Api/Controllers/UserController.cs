using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stronger.Application.UseCases.User.Commands;

namespace Stronger.Api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateNewUserCommand cmd, CancellationToken cancellationToken)
        {
            Guid response = await mediator.Send(cmd);
            return Ok(response);
        }
    }
}
