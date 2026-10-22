using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;
using MiroIntegration.Infrastructure.Persistence;

namespace MiroIntegration.Infrastructure.Repositories;

public class DesignDefinitionRepository(MiroDbContext db)
{
    public async Task<List<DesignDefinition>> ListAsync(CancellationToken ct) => await db.DesignDefinitions.ToListAsync(ct);
    public async Task<DesignDefinition> CreateAsync(CancellationToken ct, DesignDefinition entity) { db.DesignDefinitions.Add(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task<DesignDefinition?> GetByIdAsync(CancellationToken ct, Guid id) => await db.DesignDefinitions.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<DesignDefinition> UpdateAsync(CancellationToken ct, DesignDefinition entity) { db.DesignDefinitions.Update(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.DesignDefinitions.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
