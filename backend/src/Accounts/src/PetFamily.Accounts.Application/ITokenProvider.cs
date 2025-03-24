using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Domain.Entities;

namespace PetFamily.Accounts.Application;

public interface ITokenProvider
{
    Task<string> GenerateAccessToken(User user);
}