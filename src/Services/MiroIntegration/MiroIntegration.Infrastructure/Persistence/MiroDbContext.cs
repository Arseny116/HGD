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
    public DbSet<Project> Projects => Set<Project>();

    public DbSet<Pdf> Pdfs => Set<Pdf>();

    
}