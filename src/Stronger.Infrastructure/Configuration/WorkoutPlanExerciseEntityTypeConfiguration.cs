using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stronger.Domain.Entities;

namespace Stronger.Infrastructure.Configuration;

public class WorkoutPlanExerciseEntityTypeConfiguration : IEntityTypeConfiguration<WorkoutPlanExerciseEntity>
{
    public void Configure(EntityTypeBuilder<WorkoutPlanExerciseEntity> builder)
    {
        builder
            .HasKey(e => new { e.ExerciseId, e.WorkoutPlanId });

        builder
            .HasOne<WorkoutPlanEntity>()
            .WithMany()
            .HasForeignKey(e => e.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<ExerciseEntity>()
            .WithMany()
            .HasForeignKey(e => e.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
