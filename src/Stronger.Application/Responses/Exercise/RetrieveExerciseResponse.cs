using System;
using Stronger.Domain.Enums;

namespace Stronger.Application.Responses.Exercise;

public class RetrieveExerciseResponse : ExerciseResponseBase
{
    public String Name { get; set; }
    public String Description { get; set; }
    public String? ImagePath { get; set; }
    public MuscleGroup PrimaryMuscleGroup { get; set; }
    public MuscleGroup? SecondaryMuscleGroup { get; set; }
    public ExerciseType ExerciseType { get; set; }
    public ForceType ForceType { get; set; }

    public RetrieveExerciseResponse()
    {
        Name = String.Empty;
        Description = String.Empty;
    }
}
