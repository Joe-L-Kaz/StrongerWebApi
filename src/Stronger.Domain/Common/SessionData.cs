using System;

namespace Stronger.Domain.Common;

public class SessionData
{
    public List<Exercise> Exercises { get; set; }

    public SessionData()
    {
        Exercises = new();
    }
}
