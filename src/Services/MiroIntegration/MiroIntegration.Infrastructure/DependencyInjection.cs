using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using MiroIntegration.Application.Abstractions;
using MiroIntegration.Infrastructure.Repositories;
using MiroIntegration.Infrastructure.Storage;
using Microsoft.Extensions.Options;
using Amazon.S3;

using MiroIntegration.Infrastructure.Security;

namespace MiroIntegration.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MiroDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.Configure<S3Options>(configuration.GetSection("S3"));
        services.AddSingleton<IAmazonS3>(serviceProvider =>
        {
            var options = serviceProvider
                .GetRequiredService<IOptions<S3Options>>()
                .Value;

            return new AmazonS3Client(
                options.AccessKey,
                options.SecretKey,
                new AmazonS3Config
                {
                    ServiceURL = options.ServiceUrl,
                    ForcePathStyle = options.ForcePathStyle,
                    AuthenticationRegion = options.Region
                });
        });
        services.AddScoped<IPdfFileStorage, S3FileStorage>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<UserRepository>();
        services.AddScoped<ProjectRepository>();
        services.AddScoped<IPdfRepository, PdfRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, JwtTokenService>();
        return services;
    }

    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    if (string.IsNullOrWhiteSpace(context.Token))
                        context.Token = context.Request.Cookies["miro_auth"];

                    return Task.CompletedTask;
                }
            };
        });
        services.AddAuthorization();

        return services;
    }
}