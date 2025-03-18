namespace PetFamily.Volunteers.Contracts.Requests.Volunteer.PetPhotos;

//Планируется, что будут приходить уже полные названия файлов(гуид + расширение)
public record DeletePetPhotoRequest(IEnumerable<string> PhotoNames);   