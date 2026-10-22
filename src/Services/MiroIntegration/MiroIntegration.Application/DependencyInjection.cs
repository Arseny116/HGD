using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
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
        });
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}
