using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Commands.ChangePetHelpStatus;

public record ChangePetHelpStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    string HelpStatus) : ICommand;