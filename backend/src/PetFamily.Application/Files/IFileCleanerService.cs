namespace PetFamily.Application.Files;

public interface IFileCleanerService
{
    Task ProcessAsync(CancellationToken cancellationToken);
}