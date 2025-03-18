using PetFamily.Core.Dto.Shared;

namespace PetFamily.Core.Dto.VolunteerDto;

public class VolunteerDto
{
    public bool IsDeleted { get; init; }
    public Guid Id { get; init; }
    
    public string FirstName { get; init; } = string.Empty;
    
    public string LastName { get; init; } = string.Empty;
    
    public string Surname { get; init; } = string.Empty;
    
    public string Phone { get; init; } = string.Empty;
    
    public string Email { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;

    public int YearsOfExperience { get; init; } = default;
    
    public SocialWebDto[] SocialWebs { get; init; } = [];
    
    public TransferDetailDto[] TransferDetails { get; init; } = [];
    
    public PetDto.PetDto[] Pets { get; init; } = [];
}