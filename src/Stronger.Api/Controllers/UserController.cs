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

        [HttpPut]
        [ActionName("SetTrainingDays")]
        public Task<IActionResult> UpdateTrainingDaysAsync([FromQuery] short bitMask, CancellationToken cancellationToken)
        {
            SetTrainingDaysCommand command = new(bitMask);
            return this.SendAsync(command, cancellationToken);
        }
    }
}
