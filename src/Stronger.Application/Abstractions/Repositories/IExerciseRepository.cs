using System;
using Stronger.Domain.Entities;

namespace Stronger.Application.Abstractions.Repositories;

public interface IExerciseRepository : IRepositoryBase<ExerciseEntity>
{
    public Task<bool> AnyAsync(String nameNormalized, CancellationToken cancellationToken);
}
