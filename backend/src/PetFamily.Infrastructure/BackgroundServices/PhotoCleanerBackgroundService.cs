using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Files;

namespace PetFamily.Infrastructure.BackgroundServices;

public class PhotoCleanerBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public PhotoCleanerBackgroundService(
        IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<PhotoCleanerBackgroundService>>();
        logger.LogInformation("PhotoCleanerBackgroundService is starting.");
        

        var fileProvider = scope.ServiceProvider.GetRequiredService<IFileCleanerService>();
        while (!stoppingToken.IsCancellationRequested)
        {
            await fileProvider.ProcessAsync(stoppingToken);
        }
    }
}
