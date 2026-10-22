using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;
using MiroIntegration.Infrastructure.Persistence;

namespace MiroIntegration.Infrastructure.Repositories;

public class PlayerDefinitionRepository(MiroDbContext db)
{
    public async Task<List<PlayerDefinition>> ListAsync(CancellationToken ct) => await db.PlayerDefinitions.ToListAsync(ct);
    public async Task<PlayerDefinition> CreateAsync(CancellationToken ct, PlayerDefinition entity) { db.PlayerDefinitions.Add(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task<PlayerDefinition?> GetByIdAsync(CancellationToken ct, Guid id) => await db.PlayerDefinitions.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<PlayerDefinition> UpdateAsync(CancellationToken ct, PlayerDefinition entity) { db.PlayerDefinitions.Update(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.PlayerDefinitions.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
