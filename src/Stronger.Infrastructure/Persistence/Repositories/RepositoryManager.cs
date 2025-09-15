using System;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;

namespace Stronger.Infrastructure.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly IStrongerDbContext _context;
    private readonly IUserRepository _users;
    private readonly IExerciseRepository _exercises;

    public RepositoryManager(IStrongerDbContext context)
    {
        _context = context;
        _users = new UserRepository(_context);
        _exercises = new ExerciseRepository(_context);
    }

    IUserRepository IRepositoryManager.Users => _users;

    IExerciseRepository IRepositoryManager.Exercises => _exercises;

    async Task IRepositoryManager.SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync();
    }
}
