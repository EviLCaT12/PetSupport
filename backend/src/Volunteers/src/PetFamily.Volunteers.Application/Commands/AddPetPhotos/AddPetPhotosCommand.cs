using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.PetDto;

namespace PetFamily.Volunteers.Application.Commands.AddPetPhotos;

public record AddPetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadPhotoDto> Photos) : ICommand;