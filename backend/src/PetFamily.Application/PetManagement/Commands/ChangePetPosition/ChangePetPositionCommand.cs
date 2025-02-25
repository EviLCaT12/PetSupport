using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Commands.ChangePetPosition;

public record ChangePetPositionCommand(
    Guid VolunteerId,
    Guid PetId,
    int PetPosition) : ICommand;