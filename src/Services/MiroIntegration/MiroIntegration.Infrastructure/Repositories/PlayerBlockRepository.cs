using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;
using MiroIntegration.Infrastructure.Persistence;

namespace MiroIntegration.Infrastructure.Repositories;

public class PlayerBlockRepository(MiroDbContext db)
{
    public async Task<List<PlayerBlock>> ListAsync(CancellationToken ct) => await db.PlayerBlocks.ToListAsync(ct);
    public async Task<PlayerBlock> CreateAsync(CancellationToken ct, PlayerBlock entity) { db.PlayerBlocks.Add(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task<PlayerBlock?> GetByIdAsync(CancellationToken ct, Guid id) => await db.PlayerBlocks.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<PlayerBlock> UpdateAsync(CancellationToken ct, PlayerBlock entity) { db.PlayerBlocks.Update(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.PlayerBlocks.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
