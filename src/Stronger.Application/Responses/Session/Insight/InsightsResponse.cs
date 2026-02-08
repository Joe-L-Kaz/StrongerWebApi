using System;

namespace Stronger.Application.Responses.Session.Insight;

public class InsightsResponse
{
    public Dictionary<String, List<SetDataPlot>> Plots { get; set; }

    public InsightsResponse()
    {
        Plots = new();
    }

    public class SetDataPlot
    {
        public DateOnly Date {get; set;}
        public float MaxWeight {get; set;}
    }
}
