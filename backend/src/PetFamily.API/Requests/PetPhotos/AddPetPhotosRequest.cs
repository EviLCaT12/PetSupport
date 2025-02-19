namespace PetFamily.API.Requests.PetPhotos;

public record AddPetPhotosRequest(Guid PetId ,IFormFileCollection Photos);