using MiroIntegration.Domain.Models;

namespace MiroIntegration.Application.Abstractions;

public interface IUserRepository
{
    Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
}

public interface ICorePillarRepository
{
    Task<CorePillar?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(CorePillar corePillar, CancellationToken cancellationToken);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}

public interface ITokenService
{
    string CreateToken(User user);
}

public interface IPdfGenerationQueue
{
    ValueTask EnqueueAsync(Guid corePillarId, CancellationToken cancellationToken);
}