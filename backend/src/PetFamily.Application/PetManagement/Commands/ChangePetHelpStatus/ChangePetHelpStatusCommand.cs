using PetFamily.Application.Abstractions;
using PetFamily.Domain.PetContext.Entities;

namespace PetFamily.Application.PetManagement.Commands.ChangePetHelpStatus;

public record ChangePetHelpStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    string HelpStatus) : ICommand;