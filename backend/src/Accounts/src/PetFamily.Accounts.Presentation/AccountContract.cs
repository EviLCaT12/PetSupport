
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Infrastructure.Managers;

namespace PetFamily.Accounts.Presentation;

public class AccountContract : IAccountContract
{
    private readonly PermissionManager _permissionManager;
    private readonly UserManager<User> _userManager;

    public AccountContract(PermissionManager permissionManager, UserManager<User> userManager)
    {
        _permissionManager = permissionManager;
        _userManager = userManager;
    }
    public async Task<HashSet<string>> GetUserPermissionCodes(Guid userId)
    {
        return await _permissionManager.GetUserPermissionCodes(userId);
    }

    /*
     * true - если пользователь уже является волонтёром
     * false - если пользователь не является волотёром
     */
    public async Task<bool> IsUserAlreadyVolunteer(Guid userId, CancellationToken cancellationToken = default)
    {
        //Пытаемся получить юзера, расчитываем, что userId тут никогда не нул и такой пользователь и правда существует
        var user = await _userManager.Users.FirstOrDefaultAsync(cancellationToken);

        return user!.VolunteerAccount is not null;
    }
}