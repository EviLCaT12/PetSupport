
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application.Commands.BanUserForSendVolunteerRequest;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Infrastructure.Managers;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Accounts.Presentation;

public class AccountContract : IAccountContract
{
    private readonly PermissionManager _permissionManager;
    private readonly UserManager<User> _userManager;
    private readonly BanUserForSendVolunteerRequestHandler _handler;

    public AccountContract(
        PermissionManager permissionManager, 
        UserManager<User> userManager,
        BanUserForSendVolunteerRequestHandler handler)
    {
        _permissionManager = permissionManager;
        _userManager = userManager;
        _handler = handler;
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

    /*
     * true - если пользователь снова может отправлять заявку (прошло 7 дней с бана)
     * false - если пользователь не может отправлять заявку (не прошло 7 дней с бана)
     */
    public async Task<Result<bool, ErrorList>> IsUserCanSendVolunteerRequests(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .Include(u => u.ParticipantAccount)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user is null)
            return Errors.General.ValueNotFound(userId).ToErrorList();

        return DateTime.UtcNow > user.ParticipantAccount!.BanForSendingRequestUntil;
    }

    public async Task<UnitResult<ErrorList>> BanUserToSendVolunteerRequest(BanUserToSendVolunteerRequestRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _handler.HandleAsync(
            new BanUserForSendVolunteerRequestCommand(request.UserId),
            cancellationToken);

        if (result.IsFailure)
            return result.Error;

        return UnitResult.Success<ErrorList>();
    }
}