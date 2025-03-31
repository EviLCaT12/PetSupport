using PetFamily.Core.Dto.AccountDto;

namespace PetFamily.Accounts.Application.DataBase;

public interface IReadDbContext
{
    IQueryable<UserDto> Accounts { get; }
}