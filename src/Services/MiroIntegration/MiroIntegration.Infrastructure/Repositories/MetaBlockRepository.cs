using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;
using MiroIntegration.Infrastructure.Persistence;

namespace MiroIntegration.Infrastructure.Repositories;

public class MetaBlockRepository(MiroDbContext db)
{
    public async Task<List<MetaBlock>> ListAsync(CancellationToken ct) => await db.MetaBlocks.ToListAsync(ct);
    public async Task<MetaBlock> CreateAsync(CancellationToken ct, MetaBlock entity) { db.MetaBlocks.Add(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task<MetaBlock?> GetByIdAsync(CancellationToken ct, Guid id) => await db.MetaBlocks.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<MetaBlock> UpdateAsync(CancellationToken ct, MetaBlock entity) { db.MetaBlocks.Update(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.MetaBlocks.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
