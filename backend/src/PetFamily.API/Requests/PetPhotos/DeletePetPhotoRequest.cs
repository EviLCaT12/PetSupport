namespace PetFamily.API.Requests.PetPhotos;

//Планируется, что будут приходить уже полные названия файлов(гуид + расширение)
public record DeletePetPhotoRequest(IEnumerable<string> PhotoNames);