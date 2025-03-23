using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetFamily.Accounts.Domain.Entitues.AccountEntitites;
using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.Accounts.Domain.Entitues;

public class User : IdentityUser<Guid>
{
    private List<Role> _roles = [];
    
    public IReadOnlyList<Role> Roles => _roles; 
    // public Fio FullName { get; set; } = null!;
    //
    // public Photo Photo { get; set; } = null!;
    //
    // public IReadOnlyList<SocialWeb> SocialWebs { get; set; } = [];
    
    public AdminAccount? AdminAccount { get; set; }
    
    public ParticipantAccount? Participant { get; set; }
    
    public VolunteerAccount? VolunteerAccount { get; set; }
    
    public static Result<User, Error> CreateParticipant(
        string userName,
        string email,
        Role role)
    {
        var defaultAcc = new User()
        {
            UserName = userName,
            Email = email,
            _roles =  [role]
        };
        return defaultAcc;
    }
    
}