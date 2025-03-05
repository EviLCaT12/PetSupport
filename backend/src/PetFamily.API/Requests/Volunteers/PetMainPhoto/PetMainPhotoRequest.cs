using PetFamily.Application.PetManagement.Commands.MainPetPhoto;

namespace PetFamily.API.Requests.Volunteers.PetMainPhoto;

public record PetMainPhotoRequest(string PhotoPath)
{
    public PetMainPhotoCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, PhotoPath);
}