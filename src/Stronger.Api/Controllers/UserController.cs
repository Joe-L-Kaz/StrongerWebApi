using Microsoft.AspNetCore.Mvc;

using MediatR;

using Stronger.Application.UseCases.User.Commands;
using Stronger.Domain.Responses;
using Microsoft.AspNetCore.Authorization;

namespace Stronger.Api.Controllers
{
    public class UserController(IMediator mediator) : BaseController(mediator)
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAsync(CreateNewUserCommand cmd, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(cmd);
            return await this.SendAsync(cmd, cancellationToken);
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("Authenticate")]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticateUserCommand cmd, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(cmd);
            return await this.SendAsync(cmd, cancellationToken);
        }
    }
}
