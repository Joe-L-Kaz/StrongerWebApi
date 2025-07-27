using System;

namespace Stronger.Application.Abstractions.Repositories;

public interface IRepositoryManager
{
    public IUserRepository Users { get; }
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}
