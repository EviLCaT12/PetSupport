using PetFamily.Core.Dto.PetDto;
using PetFamily.Core.Dto.Shared;

namespace PetFamily.Core.Dto.AccountDto;

public class UserDto
{
    public Guid Id { get; init; }
    
    public string FirstName { get; init; } = string.Empty;
    
    public string LastName { get; init; } = string.Empty;
    
    public string Surname { get; init; } = string.Empty;
    
    public PhotoDto? UserPhoto { get; init; } = null;
    
    public SocialWebDto[]? SocialWebs { get; init; } = [];
    
    public AdminAccountDto? Admin { get; init; } = null;
    
    public ParticipantAccountDto? Participant { get; init; } = null;
    
    public VolunteerAccountDto? Volunteer { get; init; } = null;
    
}