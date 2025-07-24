using System;
using Stronger.Domain.Entities;

namespace Stronger.Application.Abstractions.Repositories;

public interface IUserRepository : IRepositoryBase<UserEntity>
{
    Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}