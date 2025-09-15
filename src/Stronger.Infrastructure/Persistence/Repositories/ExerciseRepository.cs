using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
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
    async Task IRepositoryBase<ExerciseEntity, long>.AddAsync(ExerciseEntity entity, CancellationToken cancellationToken)
    {
        await _context.Exercises.AddAsync(entity, cancellationToken);
    }

    async Task<bool> IExerciseRepository.AnyAsync(String nameNormalized, CancellationToken cancellationToken)
    {
        return await _context.Exercises.AnyAsync(e => e.NameNormalized == nameNormalized, cancellationToken);
    }

    void IRepositoryBase<ExerciseEntity, long>.Delete(ExerciseEntity entity)
    {
        _context.Exercises.Remove(entity);
    }

    async Task<IEnumerable<ExerciseEntity>> IRepositoryBase<ExerciseEntity, long>.ListAsync(Expression<Func<ExerciseEntity, bool>>? predicate, CancellationToken cancellationToken)
    {
        return predicate is null
            ? await _context.Exercises.ToListAsync(cancellationToken)
            : await _context.Exercises.Where(predicate).ToListAsync(cancellationToken);
    }

    async Task<ExerciseEntity?> IRepositoryBase<ExerciseEntity, long>.GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.Exercises.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
