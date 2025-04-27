namespace PetFamily.Accounts.Domain.Entities.AccountEntitites;

public class ParticipantAccount
{
    public const string Participant = nameof(Participant);
    
    private ParticipantAccount() { }
    public ParticipantAccount(User user)
    {
        Id = Guid.NewGuid();
        User = user;
    }
    public Guid Id { get; set; }
    
    public User User { get; set; }
    
    public DateTime BanForSendingRequestUntil { get; set; }
    public List<Guid>? FavoritePets { get; set; } = [];
}