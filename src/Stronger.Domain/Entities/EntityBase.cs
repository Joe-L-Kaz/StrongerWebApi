using System;

namespace Stronger.Domain.Entities;

public abstract class EntityBase<T> where T : struct
{
    T Id { get; set; }

    DateTime CreatedAt { get; set; }

    public EntityBase()
    {
        CreatedAt = DateTime.UtcNow;
    }
}
