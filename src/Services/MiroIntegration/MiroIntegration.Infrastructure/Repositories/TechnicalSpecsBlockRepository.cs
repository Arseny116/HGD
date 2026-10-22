using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;
using MiroIntegration.Infrastructure.Persistence;

namespace MiroIntegration.Infrastructure.Repositories;

public class TechnicalSpecsBlockRepository(MiroDbContext db)
{
    public async Task<List<TechnicalSpecsBlock>> ListAsync(CancellationToken ct) => await db.TechnicalSpecsBlocks.ToListAsync(ct);
    public async Task<TechnicalSpecsBlock> CreateAsync(CancellationToken ct, TechnicalSpecsBlock entity) { db.TechnicalSpecsBlocks.Add(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task<TechnicalSpecsBlock?> GetByIdAsync(CancellationToken ct, Guid id) => await db.TechnicalSpecsBlocks.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<TechnicalSpecsBlock> UpdateAsync(CancellationToken ct, TechnicalSpecsBlock entity) { db.TechnicalSpecsBlocks.Update(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.TechnicalSpecsBlocks.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
