using System;
using Stronger.Domain.Enums;

namespace Stronger.Domain.Entities;

public class ExerciseEntity : EntityBase<long>
{
    public String Name { get; set; }
    public String NameNormalized { get; set; }
    public String Description { get; set; }
    public String? ImagePath { get; set; }
    public MuscleGroup PrimaryMuscleGroup { get; set; }
    public MuscleGroup? SecondaryMuscleGroup { get; set; }
    public ExerciseType ExerciseType { get; set; }
    public ForceType ForceType { get; set; }

    public ExerciseEntity()
    {
        Name = String.Empty;
        NameNormalized = String.Empty;
        Description = String.Empty;
    }
}
