using PetFamily.Application.Dto.Shared;

namespace PetFamily.Application.Volunteers.UpdateSocialWeb;

public record UpdateVolunteerSocialWebCommand(Guid VolunteerId, IEnumerable<SocialWebDto> NewSocialWebs);