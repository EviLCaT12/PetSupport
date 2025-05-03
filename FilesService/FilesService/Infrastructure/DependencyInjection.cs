using Amazon.S3;
using FilesService.Infrastructure.Options;
using FilesService.MongoDataAccess;
using MongoDB.Driver;

namespace FilesService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddFilesProviders()
            .AddS3Storage(configuration)
            .AddMongoDb(configuration)
            .AddDbContext()
            .AddRepositories();
        
        return services;
    }

    private static IServiceCollection AddFilesProviders(this IServiceCollection services)
    {
        services.AddScoped<IFileProvider, FileProvider>();
        
        return services;
    }

    private static IServiceCollection AddS3Storage(this IServiceCollection services, IConfiguration configuration)
    {
        var minioOption = configuration.GetSection(MinioOption.MINIO).Get<MinioOption>()
            ?? throw new ApplicationException("Missing minio option");
        
        services.AddSingleton<IAmazonS3>(_ =>
        {
            var config = new AmazonS3Config()
            {
                ServiceURL = minioOption.Endpoint,
                ForcePathStyle = minioOption.ForcePathStyle,
            };

            return new AmazonS3Client(minioOption.AccessKey, minioOption.SecretKey, config);
        });
        
        return services;
    }

    private static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMongoClient>(_ =>
            new MongoClient(configuration.GetConnectionString("MongoConnection")));

        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddScoped<FileMongoDbContext>();
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IFileRepository, FileRepository>();
        
        return services;
    }
}