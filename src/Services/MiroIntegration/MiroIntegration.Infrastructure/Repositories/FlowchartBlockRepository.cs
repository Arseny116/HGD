using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;
using MiroIntegration.Infrastructure.Persistence;

namespace MiroIntegration.Infrastructure.Repositories;

public class FlowchartBlockRepository(MiroDbContext db)
{
    public async Task<List<FlowchartBlock>> ListAsync(CancellationToken ct) => await db.FlowchartBlocks.ToListAsync(ct);
    public async Task<FlowchartBlock> CreateAsync(CancellationToken ct, FlowchartBlock entity) { db.FlowchartBlocks.Add(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task<FlowchartBlock?> GetByIdAsync(CancellationToken ct, Guid id) => await db.FlowchartBlocks.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<FlowchartBlock> UpdateAsync(CancellationToken ct, FlowchartBlock entity) { db.FlowchartBlocks.Update(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.FlowchartBlocks.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
