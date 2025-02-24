using PetFamily.Application.Dto.Shared;

namespace PetFamily.Application.PetManagement.UseCases.UpdateSocialWeb;

public record UpdateVolunteerSocialWebCommand(Guid VolunteerId, IEnumerable<SocialWebDto> NewSocialWebs);