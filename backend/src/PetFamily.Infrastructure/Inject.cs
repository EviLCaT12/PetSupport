using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Application.DataBase;
using PetFamily.Application.Files;
using PetFamily.Application.Messaging;
using PetFamily.Application.Providers;
using PetFamily.Application.Species;
using PetFamily.Application.Volunteers;
using PetFamily.Infrastructure.BackgroundServices;
using PetFamily.Infrastructure.Files;
using PetFamily.Infrastructure.MessageQueues;
using PetFamily.Infrastructure.Options;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;
using FileInfo = PetFamily.Application.Files.FileInfo;

namespace PetFamily.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<IVolunteersRepository, VolunteerRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        services.AddMinio(configuration);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddHostedService<PhotoCleanerBackgroundService>();
        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, FileCleanerMessageQueue<IEnumerable<FileInfo>>>();
        services.AddScoped<IFileCleanerService, FilesCleanerService>();
        
        return services;
    }

    private static IServiceCollection AddMinio(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(
            configuration.GetSection("Minio"));
        
        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing " + MinioOptions.MINIO);
            
            options.WithEndpoint(minioOptions.Endpoint);

            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);

            options.WithSSL(minioOptions.WithSsl);
        });

        services.AddScoped<IFileProvider, MinioProvider>();
        
        return services;
    }
}