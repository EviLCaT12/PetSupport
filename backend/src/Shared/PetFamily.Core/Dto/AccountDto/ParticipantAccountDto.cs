namespace PetFamily.Core.Dto.AccountDto;

public class ParticipantAccountDto
{
    public Guid Id { get; init; }
    
    public Guid UserId { get; init; }
    
    public Guid[]? FavoritePets { get; init; } = [];
}