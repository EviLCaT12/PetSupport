using PetFamily.Application.PetManagement.Commands.DeletePetPhotos;

namespace PetFamily.API.Requests.Volunteers.PetPhotos;

//Планируется, что будут приходить уже полные названия файлов(гуид + расширение)
public record DeletePetPhotoRequest(IEnumerable<string> PhotoNames)
{
    public DeletePetPhotosCommand ToCommand(Guid volunteerId, Guid petId)
        => new DeletePetPhotosCommand(
            volunteerId,
            petId,
            PhotoNames);
}