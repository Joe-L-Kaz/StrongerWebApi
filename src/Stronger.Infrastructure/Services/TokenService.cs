using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


using Stronger.Application.Common.Interfaces;

namespace Stronger.Infrastructure.Services;

internal sealed class TokenService : ITokenService
{
    private IConfiguration _config;
    public TokenService(IConfiguration config)
    {
        _config = config;
    }
    public String GenerateToken(Guid id, String forename, String surname, DateOnly dob, String email)
    {
        String issuer = _config["JwtConfig:Issuer"]!;
        String audience = _config["JwtConfig:Issuer"]!;
        String key = _config["JwtConfig:Key"]!;
        int tokenValidityMins = int.Parse(_config["JwtConfig:TokenValidityMins"]!);
        DateTime tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, forename),
                new Claim(JwtRegisteredClaimNames.FamilyName, surname),
                new Claim(JwtRegisteredClaimNames.Birthdate, dob.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email)
            }),
            Expires = tokenExpiryTimeStamp,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }

}
