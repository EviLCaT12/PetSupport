using PetFamily.Application.Abstractions;
using PetFamily.Application.Dto.Shared;

namespace PetFamily.Application.PetManagement.Commands.UpdateSocialWeb;

public record UpdateVolunteerSocialWebCommand(Guid VolunteerId, IEnumerable<SocialWebDto> NewSocialWebs) : ICommand;