using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Domain.Entities;

namespace Stronger.Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly IStrongerDbContext _context;

    public RoleRepository(IStrongerDbContext context)
    {
        _context = context;
    }

    public async Task<RoleEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Roles.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<RoleEntity>> ListAsync(Expression<Func<RoleEntity, bool>>? predicate, CancellationToken cancellationToken)
    {
        return await _context.Roles.ToListAsync(cancellationToken);
    }

    async Task IRepositoryBase<RoleEntity, int>.AddAsync(RoleEntity entity, CancellationToken cancellationToken)
    {
        await _context.Roles.AddAsync(entity, cancellationToken);
    }

    void IRepositoryBase<RoleEntity, int>.Delete(RoleEntity entity)
    {
        _context.Roles.Remove(entity);
    }
}
