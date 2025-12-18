using System;

namespace Stronger.Domain.Common;

public class SetData
{
    public int SetNumber { get; set; }
    public int? Reps { get; set; }
    public float? Weight { get; set; }
    public int? RestTimeSeconds { get; set; }
    public int? CardioTimeSeconds { get; set; }
}
