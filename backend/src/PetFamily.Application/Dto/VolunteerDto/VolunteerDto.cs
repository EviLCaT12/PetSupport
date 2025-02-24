using PetFamily.Application.Dto.Shared;

namespace PetFamily.Application.Dto.VolunteerDto;

public class VolunteerDto
{
    public Guid Id { get; init; }
    
    public string FirstName { get; init; } = string.Empty;
    
    public string LastName { get; init; } = string.Empty;
    
    public string Surname { get; init; } = string.Empty;
    
    public string Number { get; init; } = string.Empty;
    
    public string Email { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;

    public int YearsOfExperience { get; init; } = default;
    
    public string SocialWebs { get; init; } = string.Empty;
    
    public string TransferDetails { get; init; } = string.Empty;
    
    public PetDto.PetDto[] Pets { get; init; } = [];
}