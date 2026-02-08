using System;

namespace Stronger.Domain.Common;

public class SessionData
{
    public List<Exercise> Exercises { get; set; }
    public DateOnly Date {get; set;}

    public SessionData()
    {
        Exercises = new();
        Date = DateOnly.FromDateTime(DateTime.Now);
    }
}
