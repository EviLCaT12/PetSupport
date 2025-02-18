namespace PetFamily.API.Requests.PetPhotos;

//Планируется, что будут приходить уже полные названия файлов(гуид + расширение)
public record DeletePetPhotoRequest(Guid PetId, IEnumerable<string> PhotoNames);