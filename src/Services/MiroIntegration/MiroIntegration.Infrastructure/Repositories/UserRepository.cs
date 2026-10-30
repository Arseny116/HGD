using Microsoft.EntityFrameworkCore;
using MiroIntegration.Application.Abstractions;
using MiroIntegration.Domain.Models;

namespace MiroIntegration.Infrastructure.Repositories;

public class UserRepository(MiroDbContext db) : IUserRepository
{
    public async Task<List<User>> ListUsersAsync(CancellationToken ct) => await db.Users.ToListAsync(ct);
    public async Task<User> CreateAsync(CancellationToken ct, User user) { db.Users.Add(user); await db.SaveChangesAsync(ct); return user; }
    public async Task<User?> GetByIdAsync(CancellationToken ct, Guid id) => await db.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task<User?> GetByEmailAsync(CancellationToken ct, string email) => await db.Users.FirstOrDefaultAsync(x => x.Email == email, ct);
    public Task<User?> FindByIdAsync(Guid id, CancellationToken ct) => GetByIdAsync(ct, id);
    public Task<User?> FindByEmailAsync(string email, CancellationToken ct) => GetByEmailAsync(ct, email);
    public async Task AddAsync(User user, CancellationToken ct) { db.Users.Add(user); await db.SaveChangesAsync(ct); }
    public async Task<User> UpdateAsync(CancellationToken ct, User user) { db.Users.Update(user); await db.SaveChangesAsync(ct); return user; }
    public async Task DeleteAsync(CancellationToken ct, Guid id) => await db.Users.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
}
