using MiroIntegration.Domain.Models;

namespace MiroIntegration.Application.Abstractions;

public interface IUserRepository
{
    Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
}

public interface IProjectRepository
{
    Task<Project?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Project Project, CancellationToken cancellationToken);
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

