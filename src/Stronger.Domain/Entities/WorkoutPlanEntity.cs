using System;

namespace Stronger.Domain.Entities;

public class WorkoutPlanEntity : EntityBase<long>
{
    public String Name { get; set; }
    public Guid? CustomerId { get; set; }

    public WorkoutPlanEntity()
    {
        Name = "";
    }
}
