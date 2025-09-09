using System;
using Stronger.Domain.Entities;

namespace Stronger.Application.Abstractions.Repositories;

public interface IUserRepository : IRepositoryBase<UserEntity, Guid>
{
    Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}