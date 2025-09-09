using System;

namespace Stronger.Application.Responses.User;

public class AuthenticateUserResponse
{
    public String AccessToken { get; set; }

    public AuthenticateUserResponse()
    {
        AccessToken = String.Empty;
    }
}
