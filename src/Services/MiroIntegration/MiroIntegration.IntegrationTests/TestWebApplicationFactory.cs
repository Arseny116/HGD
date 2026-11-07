using System.Data.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using MiroIntegration;
using MiroIntegration.Infrastructure;

namespace MiroIntegration.IntegrationTests;

public sealed class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string AuthenticationScheme = "IntegrationTest";
    public static readonly Guid TestUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("Jwt:Key", "integration-test-secret-key-that-is-long-enough");
        builder.UseSetting("ConnectionStrings:DefaultConnection", "DataSource=:memory:");
        builder.UseSetting("S3:AccessKey", "test-access-key");
        builder.UseSetting("S3:SecretKey", "test-secret-key");
        builder.UseSetting("S3:ServiceUrl", "http://localhost:9000");
        builder.UseSetting("S3:Region", "us-east-1");

        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "integration-test-secret-key-that-is-long-enough",
                ["ConnectionStrings:DefaultConnection"] = "DataSource=:memory:",
                ["S3:AccessKey"] = "test-access-key",
                ["S3:SecretKey"] = "test-secret-key",
                ["S3:ServiceUrl"] = "http://localhost:9000",
                ["S3:Region"] = "us-east-1"
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<MiroDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<MiroDbContext>>();
            services.RemoveAll<DbConnection>();

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            services.AddSingleton<DbConnection>(connection);
            services.AddDbContext<MiroDbContext>((serviceProvider, options) =>
                options.UseSqlite(serviceProvider.GetRequiredService<DbConnection>()));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = AuthenticationScheme;
                options.DefaultChallengeScheme = AuthenticationScheme;
            }).AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                AuthenticationScheme,
                _ => { });
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            var connection = Services.GetService<DbConnection>();
            connection?.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<MiroDbContext>();
        database.Database.EnsureCreated();

        return host;
    }
}
