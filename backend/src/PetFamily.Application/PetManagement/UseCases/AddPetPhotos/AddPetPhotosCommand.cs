using PetFamily.Application.Dto.PetDto;

namespace PetFamily.Application.PetManagement.UseCases.AddPetPhotos;

public record AddPetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadPhotoDto> Photos);