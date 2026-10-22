using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;
using MiroIntegration.Infrastructure.Persistence;

namespace MiroIntegration.Infrastructure.Repositories;

public class FlowchartEdgeRepository(MiroDbContext db)
{
    public async Task<List<FlowchartEdge>> ListAsync(CancellationToken ct) => await db.FlowchartEdges.ToListAsync(ct);
    public async Task<FlowchartEdge> CreateAsync(CancellationToken ct, FlowchartEdge entity) { db.FlowchartEdges.Add(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task<FlowchartEdge?> GetByIdAsync(CancellationToken ct, Guid id) => await db.FlowchartEdges.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<FlowchartEdge> UpdateAsync(CancellationToken ct, FlowchartEdge entity) { db.FlowchartEdges.Update(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.FlowchartEdges.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
