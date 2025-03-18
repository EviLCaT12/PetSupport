namespace PetFamily.Core.Files;

public interface IFileCleanerService
{
    Task ProcessAsync(CancellationToken cancellationToken);
}