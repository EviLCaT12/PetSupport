using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteer.Infrastructure.DbContexts;
using PetFamily.Volunteer.Infrastructure.Options;
using PetFamily.Volunteer.Infrastructure.Repositories;
using PetFamily.Volunteers.Application;
using Minio;
using PetFamily.Core.Files;
using PetFamily.Core.Messaging;
using PetFamily.Core.Providers;
using PetFamily.Volunteer.Infrastructure.BackgroundServices;
using PetFamily.Volunteer.Infrastructure.Files;
using PetFamily.Volunteer.Infrastructure.MessageQueues;
using PetFamily.Volunteer.Infrastructure.Providers;
using FileInfo = PetFamily.Core.Files.FileInfo;
using ServiceCollectionExtensions = Minio.ServiceCollectionExtensions;

namespace PetFamily.Volunteer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext(configuration)
            .AddRepositories()
            .AddUnitOfWork()
            .AddMinio(configuration)
            .AddHostedServices()
            .AddMessageQueue()
            .AddServices();
        return services;
    }


    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
        
        services.AddScoped<IReadDbContext, ReadDbContext>(_ => 
            new ReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteersRepository, VolunteerRepository>();
        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(ModuleKey.Volunteer);
        return services;
    }

    private static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(
            configuration.GetSection("Minio"));

        ServiceCollectionExtensions.AddMinio(services, options =>
        {
            var minioOptions = configuration.GetSection("Minio").Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing minio configuration");
            
            options.WithEndpoint(minioOptions.Endpoint);

            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);

            options.WithSSL(minioOptions.WithSsl);
        });

        services.AddScoped<IFileProvider, MinioProvider>();
        
        return services;
    }
    
    private static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<PhotoCleanerBackgroundService>();
        return services;
    }
    
    private static IServiceCollection AddMessageQueue(this IServiceCollection services)
    {
        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, FileCleanerMessageQueue<IEnumerable<FileInfo>>>();
        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IFileCleanerService, FilesCleanerService>();;
        return services;
    }
}