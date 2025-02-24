namespace PetFamily.Application.PetManagement.UseCases.DeletePetPhotos;

public record DeletePetPhotosCommand(Guid VolunteerId, Guid PetId, IEnumerable<string> PhotoNames);