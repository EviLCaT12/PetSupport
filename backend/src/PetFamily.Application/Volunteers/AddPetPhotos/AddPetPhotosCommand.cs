using PetFamily.Application.Dto.PetDto;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

namespace PetFamily.Application.Volunteers.AddPetPhotos;

public record AddPetPhotosCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<CreatePhotoDto> Photos);