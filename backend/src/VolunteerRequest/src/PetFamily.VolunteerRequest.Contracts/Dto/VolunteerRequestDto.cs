using PetFamily.VolunteerRequest.Domain.Enums;
using PetFamily.VolunteerRequest.Domain.ValueObjects;

namespace PetFamily.VolunteerRequest.Contracts.Dto;

public class VolunteerRequestDto
{
    public Guid Id { get; init; }
    
    public Guid? AdminId { get; init; }
    
    public Guid UserId { get; init; }
    
    public Guid? DiscussionId { get; init; }
    
    public string FirstName { get; init; } = string.Empty;
    
    public string LastName { get; init; } = string.Empty;
    
    public string Surname { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;
    
    public string Email { get; init; } = string.Empty;
    
    public int Experience { get; init; } = 0;
    public Status Status { get; init; }
    
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    public DateTime? Rejected_Date{ get; init; }
    
    public string? RejectionComment { get; init; }  = string.Empty;
}