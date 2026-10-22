using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;
using MiroIntegration.Infrastructure.Persistence;

namespace MiroIntegration.Infrastructure.Repositories;

public class GameplayBlockRepository(MiroDbContext db)
{
    public async Task<List<GameplayBlock>> ListAsync(CancellationToken ct) => await db.GameplayBlocks.ToListAsync(ct);
    public async Task<GameplayBlock> CreateAsync(CancellationToken ct, GameplayBlock entity) { db.GameplayBlocks.Add(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task<GameplayBlock?> GetByIdAsync(CancellationToken ct, Guid id) => await db.GameplayBlocks.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<GameplayBlock> UpdateAsync(CancellationToken ct, GameplayBlock entity) { db.GameplayBlocks.Update(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.GameplayBlocks.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
