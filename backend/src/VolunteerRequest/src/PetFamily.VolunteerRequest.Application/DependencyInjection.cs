using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequest.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerRequestApplication(this IServiceCollection services)
    {
        services
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly)
            .AddCommands()
            .AddQueries();
        
        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<>), typeof(ICommandHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
    
    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}