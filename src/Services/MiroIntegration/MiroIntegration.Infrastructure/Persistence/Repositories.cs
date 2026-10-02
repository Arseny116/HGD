using Microsoft.EntityFrameworkCore;
using MiroIntegration.Application.Abstractions;
using MiroIntegration.Domain.Models;

namespace MiroIntegration.Infrastructure.Persistence;

public sealed class UserRepository(MiroDbContext db) : IUserRepository
{
    public Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken) => db.Users.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    public Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken) => db.Users.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
    public Task AddAsync(User user, CancellationToken cancellationToken) => db.Users.AddAsync(user, cancellationToken).AsTask();
}

public sealed class CorePillarRepository(MiroDbContext db) : ICorePillarRepository
{
    public Task<CorePillar?> FindByIdAsync(Guid id, CancellationToken cancellationToken) => db.CorePillars.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    public Task AddAsync(CorePillar corePillar, CancellationToken cancellationToken) => db.CorePillars.AddAsync(corePillar, cancellationToken).AsTask();
}