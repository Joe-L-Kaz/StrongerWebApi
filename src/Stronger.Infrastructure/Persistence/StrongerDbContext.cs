using System;
using Microsoft.EntityFrameworkCore;
using Stronger.Application.Common.Interfaces;
using Stronger.Domain.Entities;

namespace Stronger.Infrastructure.Persistence;

public class StrongerDbContext(DbContextOptions<StrongerDbContext> options)
    : DbContext(options), IStrongerDbContext
{
    public DbSet<UserEntity> Users { get; set; } = null!;

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();
        return base.SaveChangesAsync(cancellationToken);
    }
}
