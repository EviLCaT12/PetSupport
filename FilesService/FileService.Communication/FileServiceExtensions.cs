using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FileService.Communication;

public static class FileServiceExtensions
{
    public static IServiceCollection AddFileHttpCommunication(
        this IServiceCollection services, string url)
    {
        services.AddHttpClient<IFileService ,FileHttpClient>((sp, config) =>
        {
            config.BaseAddress = new Uri(url);
        });
        
        return services;
    }
}