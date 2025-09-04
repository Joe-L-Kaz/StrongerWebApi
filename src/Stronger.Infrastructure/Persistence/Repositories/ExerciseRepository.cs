using System;
using Microsoft.EntityFrameworkCore;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Domain.Entities;

namespace Stronger.Infrastructure.Persistence.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private IStrongerDbContext _context;
    public ExerciseRepository(IStrongerDbContext context)
    {
        _context = context;
    }
    async Task IRepositoryBase<ExerciseEntity>.AddAsync(ExerciseEntity entity, CancellationToken cancellationToken)
    {
        await _context.Exercises.AddAsync(entity, cancellationToken);
    }

    void IRepositoryBase<ExerciseEntity>.Delete(ExerciseEntity entity)
    {
        _context.Exercises.Remove(entity);
    }

    async Task<IEnumerable<ExerciseEntity>> IRepositoryBase<ExerciseEntity>.GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Exercises.ToListAsync(cancellationToken);
    }

    async Task<ExerciseEntity?> IRepositoryBase<ExerciseEntity>.GetByIdAsync(ExerciseEntity entity, CancellationToken cancellationToken)
    {
        return await _context.Exercises.FirstOrDefaultAsync(e => e.Id == entity.Id, cancellationToken);
    }
}
