using System;

namespace Stronger.Domain.Common;

public class Exercise
{
    public long Id {get; set;}
    public List<SetData> Sets {get; set;}

    public Exercise()
    {
        Sets = new();
    }
}
