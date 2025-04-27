using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Discussion.Application.Database;
using PetFamily.Discussion.Infrastructure.Contexts;
using PetFamily.Discussion.Infrastructure.Repositories;

namespace PetFamily.Discussion.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDiscussionInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDbContexts(configuration)
            .AddRepositories()
            .AddUnitOfWork();

        return services;
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
        
        services.AddScoped<IReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDiscussionRepository, DiscussionRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        
        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(ModuleKey.Discussion);
        
        return services;
    }
}