using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Contracts;
using PetFamily.Core.Models;

namespace PetFamily.Framework.Authorization;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionRequirementHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute permission)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var accountContract = scope.ServiceProvider.GetRequiredService<IAccountContract>();
        
        var userIdString = context.User.Claims
            .FirstOrDefault(c => c.Type == CustomClaims.Id)?.Value;

        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            context.Fail();
            return;
        }
        
        var permissions = await accountContract.GetUserPermissionCodes(userId);

        if (permissions.Contains(permission.Code))
        {
            context.Succeed(permission);
            return;
        }
        
        context.Fail();
    }
}