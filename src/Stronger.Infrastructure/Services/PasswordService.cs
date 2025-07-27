using Stronger.Application.Common.Interfaces;

namespace Stronger.Infrastructure.Services;

internal sealed class PasswordService : IPasswordService
{
    string IPasswordService.Hash(String password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    bool IPasswordService.Verify(String password, String hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    bool IPasswordService.Validate(string password)
    {
        return password.Length > 8;
    }
}
