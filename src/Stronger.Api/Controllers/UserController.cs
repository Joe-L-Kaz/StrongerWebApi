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
        public Task<IActionResult> CreateAsync(CreateNewUserCommand cmd, CancellationToken cancellationToken)
        {
            return this.SendAsync(cmd, cancellationToken);
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("Authenticate")]
        [Route("/api/[controller]/[action]")]
        public Task<IActionResult> AuthenticateAsync(AuthenticateUserCommand cmd, CancellationToken cancellationToken)
        {
            return this.SendAsync(cmd, cancellationToken);
        }
    }
}
