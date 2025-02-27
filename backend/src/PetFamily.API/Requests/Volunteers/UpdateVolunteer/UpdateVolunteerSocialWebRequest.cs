using PetFamily.Application.Dto.Shared;
using PetFamily.Application.PetManagement.Commands.UpdateSocialWeb;

namespace PetFamily.API.Requests.Volunteers.UpdateVolunteer;

public record UpdateVolunteerSocialWebRequest(List<SocialWebDto> SocialWebs)
{
    public UpdateVolunteerSocialWebCommand ToCommand(Guid volunteerId)
        => new UpdateVolunteerSocialWebCommand(
            volunteerId,
            SocialWebs);
}