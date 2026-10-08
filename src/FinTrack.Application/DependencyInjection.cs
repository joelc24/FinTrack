using Microsoft.Extensions.DependencyInjection;
using FinTrack.Application.Common.Behaviors;
using FluentValidation;

namespace FinTrack.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.AddMediatR(opt =>
        {
            opt.RegisterServicesFromAssembly(assembly);
            
            // Registra el comportamiento para que intercepte todos los requests
            opt.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Registra automáticamente todos los AbstractValidator del ensamblado
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
