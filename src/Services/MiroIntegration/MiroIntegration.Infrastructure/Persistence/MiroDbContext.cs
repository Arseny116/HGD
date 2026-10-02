using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;

namespace MiroIntegration.Infrastructure.Persistence;

public sealed class MiroDbContext(DbContextOptions<MiroDbContext> options): DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(); //строка
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<CorePillar> CorePillars => Set<CorePillar>();

    
}