using Microsoft.AspNetCore.Mvc;

using MediatR;

using Stronger.Application.UseCases.User.Commands;
using Stronger.Domain.Responses;

namespace Stronger.Api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : BaseController(mediator)
    {
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateNewUserCommand cmd, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(cmd);
            return await this.SendAsync(cmd, cancellationToken);
        }

        [HttpGet]
        public async Task<IActionResult> AuthenticateAsync(AuthenticateUserCommand cmd, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(cmd);
            return await this.SendAsync(cmd, cancellationToken);
        }
    }
}
