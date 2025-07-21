using System;

namespace Stronger.Application.Common.Interfaces;

public interface ITokenService
{
    public String GenerateToken(Guid id, String forename, String surname, DateOnly dob, String email);
}
