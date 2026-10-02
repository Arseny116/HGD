using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MiroIntegration.Application.Behaviors;
using MiroIntegration.Application.Features.Auth;

namespace MiroIntegration.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(RegisterCommand).Assembly;

        services.AddAutoMapper(applicationAssembly);
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(applicationAssembly);
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}
