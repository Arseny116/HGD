using MiroIntegration.Domain.Models;

namespace MiroIntegration.Application.Abstractions;



public interface IPdfRepository
{
    Task AddAsync(Pdf pdf, CancellationToken cancellationToken);
    Task<Pdf?> FindByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);
    Task<Pdf?> FindByJobIdAsync(Guid jobId, Guid userId, CancellationToken cancellationToken);
    Task<List<Pdf>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<List<Pdf>> ListByProjectIdAsync(Guid projectId, Guid userId, CancellationToken cancellationToken);
    Task UpdateAsync(Pdf pdf, CancellationToken cancellationToken);
}

public interface IPdfFileStorage
{
    Task<Stream> OpenReadAsync(string objectKey, CancellationToken cancellationToken);
}

public interface IUserRepository
{
    Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
}

public interface IProjectRepository
{
    Task<Project?> FindByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);
    Task<List<Project>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task UpdateAsync(Project project, CancellationToken cancellationToken);
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

