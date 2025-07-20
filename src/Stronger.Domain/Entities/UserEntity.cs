using System;

namespace Stronger.Domain.Entities;

public class UserEntity : EntityBase<Guid>
{
    String Forename { get; set; }    String Surname { get; set; }
    DateOnly Dob { get; set; }
    String Email { get; set; }
    String PasswordHash { get; set; }

    public UserEntity()
    {
        Forename = String.Empty;
        Surname = String.Empty;
        Email = String.Empty;
        PasswordHash = String.Empty;
    }
}
