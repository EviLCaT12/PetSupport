namespace PetFamily.Accounts.Domain.Entities;

public class RefreshSession
{
    
    public Guid Id { get; init; }
    
    public Guid UserId { get; init; }
    
    public User User { get; init; } = default!;
    
    public Guid RefreshToken { get; init; }
    
    public Guid Jti { get; init; }
    public DateTime ExpiresIn { get; init; }
    
    public DateTime CreatedOn { get; init; }
}