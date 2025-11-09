using System;

namespace Stronger.Domain.Entities;

public class UserEntity : EntityBase<Guid>
{
    public String Forename { get; set; }
    public String Surname { get; set; }
    public DateOnly Dob { get; set; }
    public String Email { get; set; }
    public String PasswordHash { get; set; }
    public int RoleId { get; set; }
    public UserEntity()
    {
        Forename = String.Empty;
        Surname = String.Empty;
        Email = String.Empty;
        PasswordHash = String.Empty;
    }
}
