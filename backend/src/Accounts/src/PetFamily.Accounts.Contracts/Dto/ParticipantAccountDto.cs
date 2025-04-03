namespace PetFamily.Accounts.Contracts.Dto;

public class ParticipantAccountDto
{
    public Guid Id { get; init; }
    
    public Guid UserId { get; init; }
    
    public Guid[]? FavoritePets { get; init; } = [];
}