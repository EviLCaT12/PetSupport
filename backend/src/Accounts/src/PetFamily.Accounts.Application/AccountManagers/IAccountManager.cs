using PetFamily.Accounts.Domain.Entities.AccountEntitites;

namespace PetFamily.Accounts.Application.AccountManagers;

public interface IAccountManager
{ 
    Task CreateParticipantAccountAsync(ParticipantAccount participantAccount);
    
    Task CreateVolunteerAccountAsync(VolunteerAccount volunteerAccount);
    
    Task CreateAdminAccountAsync(AdminAccount adminAccount);
}