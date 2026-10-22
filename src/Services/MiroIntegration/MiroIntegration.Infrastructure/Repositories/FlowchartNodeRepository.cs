using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;
using MiroIntegration.Infrastructure.Persistence;

namespace MiroIntegration.Infrastructure.Repositories;

public class FlowchartNodeRepository(MiroDbContext db)
{
    public async Task<List<FlowchartNode>> ListAsync(CancellationToken ct) => await db.FlowchartNodes.ToListAsync(ct);
    public async Task<FlowchartNode> CreateAsync(CancellationToken ct, FlowchartNode entity) { db.FlowchartNodes.Add(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task<FlowchartNode?> GetByIdAsync(CancellationToken ct, Guid id) => await db.FlowchartNodes.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<FlowchartNode> UpdateAsync(CancellationToken ct, FlowchartNode entity) { db.FlowchartNodes.Update(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.FlowchartNodes.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
