using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetFamily.Accounts.Domain.Entities.AccountEntitites;
using PetFamily.Accounts.Domain.ValueObjects;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.Accounts.Domain.Entities;

public class User : IdentityUser<Guid>
{
    private User()
    {
    }
    
    private List<Role> _roles = [];
    
    public IReadOnlyList<Role> Roles => _roles; 
    
    public Fio FullName { get; set; } = null!;
    
    public IReadOnlyList<SocialWeb>? SocialWebs { get; set; } = [];
    
    public AdminAccount? AdminAccount { get; set; }
    
    public ParticipantAccount? ParticipantAccount { get; set; }
    
    public VolunteerAccount? VolunteerAccount { get; set; }
    
    public Photo? Avatar { get; set; }
    
    public static Result<User, ErrorList> CreateParticipant(
        string userName,
        string email,
        Fio fio,
        Role role)
    {
        if (role.Name != ParticipantAccount.Participant)
            return Errors.General.ValueIsInvalid(nameof(role)).ToErrorList();
        
        return new User
        {
            UserName = userName,
            Email = email,
            FullName = fio,
            _roles = [role]
        };
    }

    public static Result<User, ErrorList> CreateAdmin(
        string userName,
        string email,
        Fio fio,
        Role role)
    {
        if (role.Name != AdminAccount.Admin)
            return Errors.General.ValueIsInvalid(nameof(role)).ToErrorList();
        
        return new User
        {
            UserName = userName,
            Email = email,
            FullName = fio,
            _roles = [role]
        };
    }
}