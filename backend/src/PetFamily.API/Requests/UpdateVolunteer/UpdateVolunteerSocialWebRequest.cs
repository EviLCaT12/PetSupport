using PetFamily.Application.Dto.Shared;

namespace PetFamily.API.Requests.UpdateVolunteer;

public record UpdateVolunteerSocialWebRequest(List<SocialWebDto> SocialWebs);