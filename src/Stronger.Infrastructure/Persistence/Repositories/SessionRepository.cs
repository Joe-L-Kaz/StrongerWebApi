using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Domain.Entities;

namespace Stronger.Infrastructure.Persistence.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly IStrongerDbContext _context;

    public SessionRepository(IStrongerDbContext context)
    {
        _context = context;
    }
    async Task IRepositoryBase<SessionEntity, long>.AddAsync(SessionEntity entity, CancellationToken cancellationToken)
    {
        await _context.Sessions.AddAsync(entity, cancellationToken);
    }

    void IRepositoryBase<SessionEntity, long>.Delete(SessionEntity entity)
    {
        _context.Sessions.Remove(entity);
    }

    async Task<SessionEntity?> IRepositoryBase<SessionEntity, long>.GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.Sessions.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    async Task<IEnumerable<SessionEntity>> IRepositoryBase<SessionEntity, long>.ListAsync(Expression<Func<SessionEntity, bool>>? predicate, CancellationToken cancellationToken)
    {
        return predicate is null
            ? await _context.Sessions.ToListAsync(cancellationToken)
            : await _context.Sessions.Where(predicate).ToListAsync(cancellationToken);
    }
}
