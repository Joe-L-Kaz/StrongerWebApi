using System;
using Stronger.Domain.Entities;

namespace Stronger.Application.Abstractions.Repositories;

public interface IRepositoryBase<T> where T : class
{
    public Task AddAsync(T entity, CancellationToken cancellationToken);
    public void Delete(T entity);
    public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    public Task<T?> GetByIdAsync(T entity, CancellationToken cancellationToken);
}
