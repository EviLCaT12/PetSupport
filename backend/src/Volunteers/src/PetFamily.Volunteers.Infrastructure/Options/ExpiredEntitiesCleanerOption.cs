namespace PetFamily.Volunteer.Infrastructure.Options;

public class ExpiredEntitiesCleanerOption
{
    public const string ExpiredEntitiesDeleteRemoveService = "ExpiredEntitiesDeleteRemoveService";

    public int DaysBeforeDelete { get; init; } = 0;

    public int WorkingCycleInHours { get; init; } = 0;
}