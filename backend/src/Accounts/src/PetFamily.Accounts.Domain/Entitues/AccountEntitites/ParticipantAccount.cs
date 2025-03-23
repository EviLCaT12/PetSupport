namespace PetFamily.Accounts.Domain.Entitues.AccountEntitites;

public class ParticipantAccount
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    private List<Guid> FavoritePets { get; set; } = [];
}