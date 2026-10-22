using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;
using MiroIntegration.Infrastructure.Persistence;

namespace MiroIntegration.Infrastructure.Repositories;

public class GameplayOutlineItemRepository(MiroDbContext db)
{
    public async Task<List<GameplayOutlineItem>> ListAsync(CancellationToken ct) => await db.GameplayOutlineItems.ToListAsync(ct);
    public async Task<GameplayOutlineItem> CreateAsync(CancellationToken ct, GameplayOutlineItem entity) { db.GameplayOutlineItems.Add(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task<GameplayOutlineItem?> GetByIdAsync(CancellationToken ct, Guid id) => await db.GameplayOutlineItems.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<GameplayOutlineItem> UpdateAsync(CancellationToken ct, GameplayOutlineItem entity) { db.GameplayOutlineItems.Update(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.GameplayOutlineItems.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
