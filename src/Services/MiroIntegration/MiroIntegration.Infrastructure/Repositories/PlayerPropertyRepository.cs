using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;
using MiroIntegration.Infrastructure.Persistence;

namespace MiroIntegration.Infrastructure.Repositories;

public class PlayerPropertyRepository(MiroDbContext db)
{
    public async Task<List<PlayerProperty>> ListAsync(CancellationToken ct) => await db.PlayerProperties.ToListAsync(ct);
    public async Task<PlayerProperty> CreateAsync(CancellationToken ct, PlayerProperty entity) { db.PlayerProperties.Add(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task<PlayerProperty?> GetByIdAsync(CancellationToken ct, Guid id) => await db.PlayerProperties.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<PlayerProperty> UpdateAsync(CancellationToken ct, PlayerProperty entity) { db.PlayerProperties.Update(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.PlayerProperties.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
