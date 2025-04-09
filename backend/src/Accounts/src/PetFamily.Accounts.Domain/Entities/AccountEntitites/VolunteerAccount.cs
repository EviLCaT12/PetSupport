using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.Accounts.Domain.Entities.AccountEntitites;

public class VolunteerAccount
{
    public const string Volunteer = nameof(Volunteer);
    
    private VolunteerAccount() { }
    public VolunteerAccount(User user, YearsOfExperience yearsOfExperience)
    {
        Id = Guid.NewGuid();
        User = user;
        Experience = yearsOfExperience;
    }
    
    public VolunteerAccount(User user, Guid volunteerId, YearsOfExperience yearsOfExperience)
    {
        Id = Guid.NewGuid();
        User = user;
        Experience = yearsOfExperience;
    }
    public Guid Id { get; set; }
    
    public Guid VolunteerId { get; set; }
    public User User { get; set; }
    public YearsOfExperience Experience { get; set; } 
    public IReadOnlyList<TransferDetails>? TransferDetails { get; set; } = [];

}