using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.Shared;

namespace PetFamily.Volunteers.Application.Commands.UpdateSocialWeb;

public record UpdateVolunteerSocialWebCommand(Guid VolunteerId, IEnumerable<SocialWebDto> NewSocialWebs) : ICommand;