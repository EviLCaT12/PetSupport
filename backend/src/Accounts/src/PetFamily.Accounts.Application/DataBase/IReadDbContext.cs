using PetFamily.Accounts.Contracts.Dto;

namespace PetFamily.Accounts.Application.DataBase;

public interface IReadDbContext
{
    IQueryable<UserDto> Accounts { get; }
}