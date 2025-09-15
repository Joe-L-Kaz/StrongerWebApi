using System;

namespace Stronger.Domain.Entities;

public abstract class EntityBase<T> where T : struct
{
    public T Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
