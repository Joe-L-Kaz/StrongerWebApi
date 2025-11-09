using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Domain.Entities;

namespace Stronger.Infrastructure.Persistence.Repositories;

public class WorkoutPlanRepository : IWorkoutPlanRepository
{
    private readonly IStrongerDbContext _context;

    public WorkoutPlanRepository(IStrongerDbContext context)
    {
        _context = context;    
    }
    async Task IRepositoryBase<WorkoutPlanEntity, long>.AddAsync(WorkoutPlanEntity entity, CancellationToken cancellationToken)
    {
        await _context.WorkoutPlans.AddAsync(entity, cancellationToken);
    }

    void IRepositoryBase<WorkoutPlanEntity, long>.Delete(WorkoutPlanEntity entity)
    {
        _context.WorkoutPlans.Remove(entity);
    }

    async Task<WorkoutPlanEntity?> IRepositoryBase<WorkoutPlanEntity, long>.GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.WorkoutPlans.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    async Task<long> IWorkoutPlanRepository.GetNextIdAsync()
    {
        var maxId = await _context.WorkoutPlans.MaxAsync(e => (long?)e.Id) ?? 0;
        return maxId + 1;
    }

    async Task<IEnumerable<WorkoutPlanEntity>> IRepositoryBase<WorkoutPlanEntity, long>.ListAsync(Expression<Func<WorkoutPlanEntity, bool>>? predicate, CancellationToken cancellationToken)
    {
        return predicate is null
            ? await _context.WorkoutPlans.ToListAsync(cancellationToken)
            : await _context.WorkoutPlans.Where(predicate).ToListAsync(cancellationToken);
    }
}
