namespace PetFamily.API.Requests;

public record ChangePetPositionRequest(Guid PetId, int NewPetPosition);