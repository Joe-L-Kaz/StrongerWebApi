using System;

namespace Stronger.Domain.Common;

public class Exercise
{
    public String ExerciseName { get; set; }
    public List<SetData> Sets {get; set;}

    public Exercise()
    {
        ExerciseName = String.Empty;
        Sets = new();
    }
}
