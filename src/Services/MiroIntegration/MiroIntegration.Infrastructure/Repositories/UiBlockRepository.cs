using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;
using MiroIntegration.Infrastructure.Persistence;

namespace MiroIntegration.Infrastructure.Repositories;

public class UiBlockRepository(MiroDbContext db)
{
    public async Task<List<UiBlock>> ListAsync(CancellationToken ct) => await db.UiBlocks.ToListAsync(ct);
    public async Task<UiBlock> CreateAsync(CancellationToken ct, UiBlock entity) { db.UiBlocks.Add(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task<UiBlock?> GetByIdAsync(CancellationToken ct, Guid id) => await db.UiBlocks.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<UiBlock> UpdateAsync(CancellationToken ct, UiBlock entity) { db.UiBlocks.Update(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.UiBlocks.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
