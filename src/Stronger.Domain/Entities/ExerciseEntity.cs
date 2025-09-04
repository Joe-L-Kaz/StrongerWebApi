using System;
using Stronger.Domain.Enums;

namespace Stronger.Domain.Entities;

public class ExerciseEntity : EntityBase<long>
{
    public String ExerciseName { get; set; }
    public String Description { get; set; }
    public String? ImagePath { get; set; }
    public MuscleGroup PrimaryMuscleGroup { get; set; }
    public MuscleGroup? SecondaryMuscleGroup { get; set; }
    public ExerciseType ExerciseType { get; set; }
    public ForceType ForceType { get; set; }

    public ExerciseEntity()
    {
        ExerciseName = String.Empty;
        Description = String.Empty;
    }
}
