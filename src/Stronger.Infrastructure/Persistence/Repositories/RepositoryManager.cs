using System;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;

namespace Stronger.Infrastructure.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly IStrongerDbContext _context;
    private readonly IUserRepository _users;

    public RepositoryManager(IStrongerDbContext context)
    {
        _context = context;
        _users = new UserRepository(_context);
    }

    IUserRepository IRepositoryManager.Users => _users;

    async Task IRepositoryManager.SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync();
    }
}
