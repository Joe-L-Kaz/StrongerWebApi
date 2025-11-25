using System;
using Stronger.Domain.Entities;

namespace Stronger.Application.Abstractions.Repositories;

public interface ISessionRepository : IRepositoryBase<SessionEntity,long> 
{

}
