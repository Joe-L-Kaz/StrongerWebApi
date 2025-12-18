using System;
using Stronger.Domain.Common;

namespace Stronger.Domain.Entities;

public class SessionEntity : EntityBase<long>
{
    public String SessionData { get; set; }
    public Guid UserId { get; set; }
    public DateOnly CompletedAt {get; set;}

    public SessionEntity()
    {
        SessionData = String.Empty;
    }
}
