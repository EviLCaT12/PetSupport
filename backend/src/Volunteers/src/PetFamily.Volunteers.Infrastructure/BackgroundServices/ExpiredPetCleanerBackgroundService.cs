using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.SharedKernel.Constants;
using PetFamily.Volunteer.Infrastructure.DeleteServices;
using PetFamily.Volunteer.Infrastructure.Options;

namespace PetFamily.Volunteer.Infrastructure.BackgroundServices;

public class ExpiredPetCleanerBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ExpiredEntitiesCleanerOption _options;

    public ExpiredPetCleanerBackgroundService(IServiceScopeFactory scopeFactory,
        IOptions<ExpiredEntitiesCleanerOption> options)
    {
        _scopeFactory = scopeFactory;
        _options = options.Value;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ExpiredPetCleanerBackgroundService>>();
            logger.LogInformation("Expired pet cleaner is running.");

            var removeService = scope.ServiceProvider.GetRequiredService<DeleteExpiredPetServices>();
            await removeService.ProcessAsync(_options.DaysBeforeDelete, stoppingToken);
            
            await Task.Delay(
                TimeSpan.FromHours(_options.WorkingCycleInHours),
                stoppingToken);
        }
    }
}