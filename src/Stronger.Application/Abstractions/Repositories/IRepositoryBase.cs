using System;
using Stronger.Domain.Entities;

namespace Stronger.Application.Abstractions.Repositories;

public interface IRepositoryBase<T> where T : class
{
    public Task AddAsync(UserEntity entity, CancellationToken cancellationToken);
    public void Delete(UserEntity entity);
    public Task<IEnumerable<UserEntity>> GetAllAsync(CancellationToken cancellationToken);
    public Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
