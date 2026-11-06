using Microsoft.EntityFrameworkCore;
using MiroIntegration.Application.Abstractions;
using MiroIntegration.Domain.Models;

namespace MiroIntegration.Infrastructure.Repositories
{
    public class ProjectRepository(MiroDbContext db) : IProjectRepository
    {
        public async Task<List<Project>> ListProjectsAsync(CancellationToken ct)
        {
            return await db.Projects.ToListAsync(ct);
        }

        public async Task<List<Project>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await db.Projects
                .Where(project => project.UserId == userId)
                .ToListAsync(cancellationToken);
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

        public async Task<Project?> GetByIdAsync(CancellationToken ct, Guid id, Guid userId)
        {
            return await db.Projects
                    .AsSplitQuery()
                    .Include(p => p.Meta)
                    .Include(p => p.TechnicalSpecs)
                    .Include(p => p.Gameplay)
                        .ThenInclude(g => g.Outline)
                    .Include(p => p.DesignDocument)
                        .ThenInclude(d => d.Definitions)
                    .Include(p => p.Flowchart)
                        .ThenInclude(f => f.Nodes)
                    .Include(p => p.Flowchart)
                        .ThenInclude(f => f.Edges)
                    .Include(p => p.Player)
                        .ThenInclude(pl => pl.Definition)
                    .Include(p => p.Player)
                        .ThenInclude(pl => pl.Properties)
                    .Include(p => p.Player)
                        .ThenInclude(pl => pl.Rewards)
                    .Include(p => p.Ui)
                        .ThenInclude(u => u.Controls)
                    .Include(p => p.PdfScope)
                    .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId, ct);
        }

        public async Task DeleteAsync(CancellationToken ct, Guid id)
        {
            await db.Projects.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
        }

        public Task<Project?> FindByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken) => GetByIdAsync(cancellationToken, id, userId);

        async Task IProjectRepository.UpdateAsync(Project project, CancellationToken cancellationToken)
        {
            db.Projects.Update(project);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task AddAsync(Project project, CancellationToken cancellationToken)
        {
            db.Projects.Add(project);
            await db.SaveChangesAsync(cancellationToken);
        }





    }
}
