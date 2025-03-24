using PetFamily.Accounts.Domain.Entitues.AccountEntitites;

namespace PetFamily.Accounts.Application.AccountManagers;

public interface IParticipantAccountManager
{
        Task CreateParticipantAccountAsync(ParticipantAccount participantAccount);
}