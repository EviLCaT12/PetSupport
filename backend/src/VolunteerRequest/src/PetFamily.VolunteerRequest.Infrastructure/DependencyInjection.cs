using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteerRequest.Application.Abstractions;
using PetFamily.VolunteerRequest.Infrastructure.DbContexts;
using PetFamily.VolunteerRequest.Infrastructure.Repositories;

namespace PetFamily.VolunteerRequest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerRequestInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddUnitOfWork()
            .AddRepositories()
            .AddContexts(configuration);
        
        return services;
    }

    private static IServiceCollection AddContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WriteContext>(_ =>
            new WriteContext(configuration.GetConnectionString(Constants.Database)!));
        
        services.AddScoped<IReadDbContext, ReadContext>(_ =>
            new ReadContext(configuration.GetConnectionString(Constants.Database)!));
        
        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(ModuleKey.VolunteerRequest);
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRequestRepository, VolunteerRequestRepository>();
        return services;
    }
    
}