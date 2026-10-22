using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;
using MiroIntegration.Infrastructure.Persistence;

namespace MiroIntegration.Infrastructure.Repositories;

public class DesignDocumentBlockRepository(MiroDbContext db)
{
    public async Task<List<DesignDocumentBlock>> ListAsync(CancellationToken ct) => await db.DesignDocumentBlocks.ToListAsync(ct);
    public async Task<DesignDocumentBlock> CreateAsync(CancellationToken ct, DesignDocumentBlock entity) { db.DesignDocumentBlocks.Add(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task<DesignDocumentBlock?> GetByIdAsync(CancellationToken ct, Guid id) => await db.DesignDocumentBlocks.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<DesignDocumentBlock> UpdateAsync(CancellationToken ct, DesignDocumentBlock entity) { db.DesignDocumentBlocks.Update(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.DesignDocumentBlocks.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
