using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Domain.Entities;

namespace Stronger.Infrastructure.Persistence.Repositories;

public class WorkoutPlanExerciseRepository : IWorkoutPlanExerciseRepository
{
    private readonly IStrongerDbContext _context;

    public WorkoutPlanExerciseRepository(IStrongerDbContext context)
    {
        _context = context;
    }

    async Task IRepositoryBase<WorkoutPlanExerciseEntity>.AddAsync(WorkoutPlanExerciseEntity entity, CancellationToken cancellationToken)
    {
        await _context.WorkoutPlanExercises.AddAsync(entity, cancellationToken);
    }

    void IRepositoryBase<WorkoutPlanExerciseEntity>.Delete(WorkoutPlanExerciseEntity entity)
    {
        _context.WorkoutPlanExercises.Remove(entity);
    }

    async Task<WorkoutPlanExerciseEntity?> IRepositoryBase<WorkoutPlanExerciseEntity>.FirstOrDefaultAsync(Expression<Func<WorkoutPlanExerciseEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.WorkoutPlanExercises.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    async Task<IEnumerable<WorkoutPlanExerciseEntity>> IRepositoryBase<WorkoutPlanExerciseEntity>.ListAsync(Expression<Func<WorkoutPlanExerciseEntity, bool>>? predicate, CancellationToken cancellationToken)
    {
        return predicate is null
            ? await _context.WorkoutPlanExercises.ToListAsync(cancellationToken)
            : await _context.WorkoutPlanExercises.Where(predicate).ToListAsync(cancellationToken);
    }
}
