using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Domain.Entitues;

namespace PetFamily.Accounts.Application;

public interface ITokenProvider
{
    Task<string> GenerateAccessToken(User user);
}