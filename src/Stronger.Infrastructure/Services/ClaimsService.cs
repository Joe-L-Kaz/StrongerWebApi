using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Stronger.Application.Common.Interfaces;
using System.Security.Claims;

namespace Stronger.Infrastructure.Services;

public class ClaimsService : IClaimsService
{
    private IHttpContextAccessor _httpContext;
    public ClaimsService(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    String IClaimsService.UserId { get => _httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new ArgumentNullException(); }
}
