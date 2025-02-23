using PetFamily.Application.Volunteers.ChangePetPosition;

namespace PetFamily.API.Requests;

public record ChangePetPositionRequest(int NewPetPosition)
{
    public ChangePetPositionCommand ToCommand(Guid volunteerId, Guid petId)
        => new ChangePetPositionCommand(
            volunteerId,
            petId,
            NewPetPosition);
}