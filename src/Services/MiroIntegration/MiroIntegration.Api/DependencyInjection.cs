
using MassTransit;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using MiroIntegration.API;
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
            x.AddConsumer<PdfResultConsumer>();
            x.AddConsumer<PdfFailureConsumer>();
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

                cfg.ReceiveEndpoint("pdf-generation-queue-answer", e =>
                {
                    e.ConfigureConsumer<PdfResultConsumer>(context);
                });
            }

            );

        });
        return services;
    }

}
