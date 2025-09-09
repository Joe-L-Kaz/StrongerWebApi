using Microsoft.AspNetCore.Mvc;

using MediatR;

using Stronger.Domain.Responses;
using Microsoft.AspNetCore.Authorization;

namespace Stronger.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public abstract class BaseController(IMediator _mediator) : ControllerBase
    {
        private protected async Task<IActionResult> SendAsync(IRequest<Response> request, CancellationToken cancellationToken)
        {
            Response response = await _mediator.Send(request, cancellationToken);

            Object? val = response.Content ?? response.Error;

            return val is null
                ? this.StatusCode(response.StatusCode)
                : this.StatusCode(response.StatusCode, val);
        }
    }
}
