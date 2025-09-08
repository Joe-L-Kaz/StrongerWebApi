using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stronger.Domain.Entities;

namespace Stronger.Infrastructure.Configuration;

public class ExerciseEntityTypeConfiguration : IEntityTypeConfiguration<ExerciseEntity>
{
    public void Configure(EntityTypeBuilder<ExerciseEntity> builder)
    {
        builder
            .HasKey(exercise => exercise.Id);

        builder
            .HasIndex(exercise => exercise.NameNormalized)
            .IsUnique();

        builder.Property(x => x.PrimaryMuscleGroup)
            .HasConversion<String>()
            .IsRequired();

        builder.Property(x => x.SecondaryMuscleGroup)
            .HasConversion<String>();

        builder.Property(x => x.ExerciseType)
            .HasConversion<String>()
            .IsRequired();

        builder.Property(x => x.ForceType)
            .HasConversion<String>()
            .IsRequired();
    }
}
