using System;
using Microsoft.EntityFrameworkCore;
using Stronger.Application.Common.Interfaces;
using Stronger.Domain.Entities;
using Stronger.Infrastructure.Configuration;

namespace Stronger.Infrastructure.Persistence;

public class StrongerDbContext(DbContextOptions<StrongerDbContext> options)
    : DbContext(options), IStrongerDbContext
{
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<ExerciseEntity> Exercises { get; set; } = null!;
    public DbSet<WorkoutPlanEntity> WorkoutPlans { get; set; } = null!;
    public DbSet<WorkoutPlanExerciseEntity> WorkoutPlanExercises { get; set; } = null!; 
    public DbSet<RoleEntity> Roles { get; set; } = null!;
    public DbSet<SessionEntity> Sessions { get; set; } = null!
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<UserEntity>());
        new ExerciseEntityTypeConfiguration().Configure(modelBuilder.Entity<ExerciseEntity>());
        new WorkoutPlanEntityTypeConfiguration().Configure(modelBuilder.Entity<WorkoutPlanEntity>());
        new WorkoutPlanExerciseEntityTypeConfiguration().Configure(modelBuilder.Entity<WorkoutPlanExerciseEntity>());
        new RoleEntityTypeConfiguration().Configure(modelBuilder.Entity<RoleEntity>());
        base.OnModelCreating(modelBuilder);
    }
}
