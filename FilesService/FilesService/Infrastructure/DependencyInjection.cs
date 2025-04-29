using Amazon.S3;

namespace FilesService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddFilesProviders()
            .AddS3Storage();
        
        return services;
    }

    private static IServiceCollection AddFilesProviders(this IServiceCollection services)
    {
        services.AddScoped<IFileProvider, FileProvider>();
        
        return services;
    }

    private static IServiceCollection AddS3Storage(this IServiceCollection services)
    {
        services.AddSingleton<IAmazonS3>(_ =>
        {
            var config = new AmazonS3Config()
            {
                ServiceURL = "http://localhost:9000",
                ForcePathStyle = true,
            };

            return new AmazonS3Client("minioadmin", "minioadmin", config);
        });
        
        return services;
    }
}