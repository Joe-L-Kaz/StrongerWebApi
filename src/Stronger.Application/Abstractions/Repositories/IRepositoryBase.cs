using System;
using System.Linq.Expressions;
using Stronger.Domain.Entities;

namespace Stronger.Application.Abstractions.Repositories;

public interface IRepositoryBase<TEntity, TKey>
where TEntity : class
where TKey : struct
{
    public Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    public void Delete(TEntity entity);
    public Task<IEnumerable<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellationToken);
    public Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken);
}
