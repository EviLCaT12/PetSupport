using Microsoft.AspNetCore.Http;

namespace PetFamily.Volunteers.Contracts.Requests.Volunteer.PetPhotos;

public record AddPetPhotosRequest(IFormFileCollection Photos);