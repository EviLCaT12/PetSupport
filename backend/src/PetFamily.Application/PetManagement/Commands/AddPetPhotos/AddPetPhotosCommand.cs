using PetFamily.Application.Abstractions;
using PetFamily.Application.Dto.PetDto;

namespace PetFamily.Application.PetManagement.Commands.AddPetPhotos;

public record AddPetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadPhotoDto> Photos) : ICommand;