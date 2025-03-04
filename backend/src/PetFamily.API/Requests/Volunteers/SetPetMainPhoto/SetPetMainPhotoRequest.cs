using PetFamily.Application.PetManagement.Commands.SetMainPetPhoto;

namespace PetFamily.API.Requests.Volunteers.SetPetMainPhoto;

public record SetPetMainPhotoRequest(string PhotoPath)
{
    public SetPetMainPhotoCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, PhotoPath);
}