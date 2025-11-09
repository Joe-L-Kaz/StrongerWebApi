using System;
using Stronger.Domain.Entities;

namespace Stronger.Application.Abstractions.Repositories;

public interface IWorkoutPlanRepository : IRepositoryBase<WorkoutPlanEntity, long>
{
    Task<long> GetNextIdAsync();
}
