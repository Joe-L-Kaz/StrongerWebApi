using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Stronger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
    }
}
