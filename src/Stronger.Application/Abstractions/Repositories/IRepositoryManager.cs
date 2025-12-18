using System;

namespace Stronger.Application.Abstractions.Repositories;

public interface IRepositoryManager
{
    public IUserRepository Users { get; }
    public IExerciseRepository Exercises { get; }
    public IWorkoutPlanRepository WorkoutPlans { get; }
    public IWorkoutPlanExerciseRepository WorkoutPlanExercises { get; }
    public IRoleRepository Roles { get; }
    public ISessionRepository Sessions { get; }
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}
