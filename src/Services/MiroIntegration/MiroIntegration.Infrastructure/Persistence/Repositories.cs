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

public sealed class ProjectRepository(MiroDbContext db) : IProjectRepository
{
    public Task<Project?> FindByIdAsync(Guid id, CancellationToken cancellationToken) => db.Projects.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    public Task AddAsync(Project Project, CancellationToken cancellationToken) => db.Projects.AddAsync(Project, cancellationToken).AsTask();
}

public s