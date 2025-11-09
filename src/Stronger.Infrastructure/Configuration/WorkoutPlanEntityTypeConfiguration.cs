using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stronger.Domain.Entities;

namespace Stronger.Infrastructure.Configuration;

public class WorkoutPlanEntityTypeConfiguration : IEntityTypeConfiguration<WorkoutPlanEntity>
{
    public void Configure(EntityTypeBuilder<WorkoutPlanEntity> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(user => user.CreatedAt)
            .IsRequired();

        builder
            .Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .HasOne<UserEntity>()
            .WithMany()
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
