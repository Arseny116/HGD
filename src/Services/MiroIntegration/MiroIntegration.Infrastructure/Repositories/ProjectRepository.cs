using Microsoft.EntityFrameworkCore;
using MiroIntegration.Application.Abstractions;
using MiroIntegration.Domain.Models;
using MiroIntegration.Infrastructure.Persistence;

namespace MiroIntegration.Infrastructure.Repositories
{
    public class ProjectRepository(MiroDbContext db) : IProjectRepository
    {
        public async Task<List<Project>> ListProjectsAsync(CancellationToken ct)
        {
            return await db.Projects.ToListAsync(ct);
        }

        public async Task<Project> UpdateAsync(CancellationToken ct, Project project)
        {
            db.Projects.Update(project);
            await db.SaveChangesAsync(ct);
            return project;
        }

        public async Task<Project> CreateAsync(CancellationToken ct, Project project)
        {
            db.Projects.Add(project);
            await db.SaveChangesAsync(ct);
            return project;
        }

        public async Task<Project?> GetByIdAsync(CancellationToken ct, Guid id)
        {
            return await db.Projects.FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task DeleteAsync(CancellationToken ct, Guid id)
        {
            await db.Projects.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
        }

        public Task<Project?> FindByIdAsync(Guid id, CancellationToken cancellationToken) => GetByIdAsync(cancellationToken, id);

        public async Task AddAsync(Project project, CancellationToken cancellationToken)
        {
            db.Projects.Add(project);
            await db.SaveChangesAsync(cancellationToken);
        }

       



    }
}
