using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;
using MiroIntegration.Infrastructure.Persistence;

namespace MiroIntegration.Infrastructure.Repositories;

public class UiControlRepository(MiroDbContext db)
{
    public async Task<List<UiControl>> ListAsync(CancellationToken ct) => await db.UiControls.ToListAsync(ct);
    public async Task<UiControl> CreateAsync(CancellationToken ct, UiControl entity) { db.UiControls.Add(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task<UiControl?> GetByIdAsync(CancellationToken ct, Guid id) => await db.UiControls.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<UiControl> UpdateAsync(CancellationToken ct, UiControl entity) { db.UiControls.Update(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.UiControls.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
