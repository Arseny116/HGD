using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MiroIntegration.Infrastructure;

public static class DatabaseMigrationExtensions
{
    public static IServiceProvider MigrateDatabase(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        scope.ServiceProvider.GetRequiredService<MiroDbContext>().Database.Migrate();
        return services;
    }
}
