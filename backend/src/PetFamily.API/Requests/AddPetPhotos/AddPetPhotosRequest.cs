namespace PetFamily.API.Requests.AddPetPhotos;

public record AddPetPhotosRequest(Guid PetId ,IFormFileCollection Photos);