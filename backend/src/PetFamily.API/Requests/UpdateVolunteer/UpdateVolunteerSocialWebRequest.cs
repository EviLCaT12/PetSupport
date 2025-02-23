using PetFamily.Application.Dto.Shared;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateSocialWeb;

namespace PetFamily.API.Requests.UpdateVolunteer;

public record UpdateVolunteerSocialWebRequest(List<SocialWebDto> SocialWebs)
{
    public UpdateVolunteerSocialWebCommand ToCommand(Guid volunteerId)
        => new UpdateVolunteerSocialWebCommand(
            volunteerId,
            SocialWebs);
}