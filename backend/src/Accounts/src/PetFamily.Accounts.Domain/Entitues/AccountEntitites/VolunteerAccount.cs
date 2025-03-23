using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.Accounts.Domain.Entitues.AccountEntitites;

public class VolunteerAccount
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public YearsOfExperience Experience { get; set; }
    public IReadOnlyList<TransferDetails> TransferDetails { get; set; } = [];

}