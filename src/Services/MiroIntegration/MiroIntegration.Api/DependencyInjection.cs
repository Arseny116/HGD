
using MassTransit;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using MiroIntegration.Services;
using static Shared.Contracts.Contracts;

namespace MiroIntegration.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    { 
        services.AddControllers();
        services.AddOpenApi();
        services.AddProblemDetails();
        services.AddScoped<PdfNotifier>();
        services.AddSignalR();

        return services;
    }

    public static IServiceCollection AddMassTransitService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
              cfg.Host(
              configuration["RabbitMQ:Host"] ?? "localhost",
              "/",
              h =>
              {
                  h.Username(
                      configuration["RabbitMQ:Username"] ?? "guest");

                  h.Password(
                      configuration["RabbitMQ:Password"] ?? "guest");
              });
            }
            );

        });
        return services;
    }
}
