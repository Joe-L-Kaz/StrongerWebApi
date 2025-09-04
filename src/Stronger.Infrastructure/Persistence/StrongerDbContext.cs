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
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<UserEntity>());
        base.OnModelCreating(modelBuilder);
    }
}
