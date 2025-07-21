using System;

namespace Stronger.Application.Common.Interfaces;

public interface IPasswordService
{
    public String Hash(String password);
    public bool Verify(String password, String hash);
    public bool Validate(String password);
}
