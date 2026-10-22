using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;
using MiroIntegration.Infrastructure.Persistence;

namespace MiroIntegration.Infrastructure.Repositories;

public class PlayerRewardRepository(MiroDbContext db)
{
    public async Task<List<PlayerReward>> ListAsync(CancellationToken ct) => await db.PlayerRewards.ToListAsync(ct);
    public async Task<PlayerReward> CreateAsync(CancellationToken ct, PlayerReward entity) { db.PlayerRewards.Add(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task<PlayerReward?> GetByIdAsync(CancellationToken ct, Guid id) => await db.PlayerRewards.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<PlayerReward> UpdateAsync(CancellationToken ct, PlayerReward entity) { db.PlayerRewards.Update(entity); await db.SaveChangesAsync(ct); return entity; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.PlayerRewards.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
