using System;
using Stronger.Application.Responses.Exercise;

namespace Stronger.Application.Responses.WorkoutPlan;

public class RetrieveWorkoutPlanResponse : WorkoutPlanResponseBase
{
    public String Name { get; set; }
    public IEnumerable<RetrieveExerciseResponse> Exercises { get; set; }

    public RetrieveWorkoutPlanResponse()
    {
        Name = String.Empty;
        Exercises = new List<RetrieveExerciseResponse>();
    }    
}
