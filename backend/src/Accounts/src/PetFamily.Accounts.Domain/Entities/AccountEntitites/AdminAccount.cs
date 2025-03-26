namespace PetFamily.Accounts.Domain.Entities.AccountEntitites;

public class AdminAccount
{
    public const string Admin = nameof(Admin);
    
    private AdminAccount()
    {}
    public AdminAccount(User user)
    {
        Id = Guid.NewGuid();
        User = user;
    }
    
    public Guid Id { get; set; }
    
    public User User { get; set; }
}