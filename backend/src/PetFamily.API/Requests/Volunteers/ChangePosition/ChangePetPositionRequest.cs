using PetFamily.Application.PetManagement.UseCases.ChangePetPosition;

namespace PetFamily.API.Requests.Volunteers.ChangePosition;

public record ChangePetPositionRequest(int NewPetPosition)
{
    public ChangePetPositionCommand ToCommand(Guid volunteerId, Guid petId)
        => new ChangePetPositionCommand(
            volunteerId,
            petId,
            NewPetPosition);
}