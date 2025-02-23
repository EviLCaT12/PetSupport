using PetFamily.Application.Volunteers.DeletePetPhotos;

namespace PetFamily.API.Requests.PetPhotos;

//Планируется, что будут приходить уже полные названия файлов(гуид + расширение)
public record DeletePetPhotoRequest(IEnumerable<string> PhotoNames)
{
    public DeletePetPhotosCommand ToCommand(Guid volunteerId, Guid petId)
        => new DeletePetPhotosCommand(
            volunteerId,
            petId,
            PhotoNames);
}