using PetFamily.Application.PetManagement.Commands.ChangePetHelpStatus;

namespace PetFamily.API.Requests.Volunteers.ChangePetHelpStatus;

public record ChangePetHelpStatusRequest(string HelpStatus)
{
    public ChangePetHelpStatusCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, HelpStatus);
}