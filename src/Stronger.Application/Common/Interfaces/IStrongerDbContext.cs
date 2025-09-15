using System;
using Microsoft.EntityFrameworkCore;
using Stronger.Domain.Entities;

namespace Stronger.Application.Common.Interfaces;

public interface IStrongerDbContext
{
    DbSet<UserEntity> Users { get; }
    DbSet<ExerciseEntity> Exercises { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
