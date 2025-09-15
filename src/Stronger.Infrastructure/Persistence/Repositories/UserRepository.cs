using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Domain.Entities;

namespace Stronger.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IStrongerDbContext _context;

    public UserRepository(IStrongerDbContext context)
    {
        _context = context;
    }

    async Task IRepositoryBase<UserEntity, Guid>.AddAsync(UserEntity entity, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(entity, cancellationToken);
    }

    void IRepositoryBase<UserEntity, Guid>.Delete(UserEntity entity)
    {
        _context.Users.Remove(entity);
    }

    async Task<IEnumerable<UserEntity>> IRepositoryBase<UserEntity, Guid>.ListAsync(Expression<Func<UserEntity, bool>>? predicate,CancellationToken cancellationToken)
    {
        return predicate is null
            ? await _context.Users.ToListAsync(cancellationToken)
            : await _context.Users.Where(predicate).ToListAsync(cancellationToken);
    }

    async Task<UserEntity?> IUserRepository.GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(e => e.Email == email, cancellationToken);
    }

    async Task<UserEntity?> IRepositoryBase<UserEntity, Guid>.GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
