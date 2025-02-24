namespace PetFamily.Application.PetManagement.UseCases.ChangePetPosition;

public record ChangePetPositionCommand(
    Guid VolunteerId,
    Guid PetId,
    int PetPosition);