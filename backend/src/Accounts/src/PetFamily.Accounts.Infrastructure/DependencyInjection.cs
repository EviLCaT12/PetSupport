using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Application.AccountManagers;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Infrastructure.Contexts;
using PetFamily.Accounts.Infrastructure.Managers;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Accounts.Infrastructure.Providers;
using PetFamily.Accounts.Infrastructure.Seeding;
using PetFamily.Core.Options;
using PetFamily.SharedKernel.Constants;
namespace PetFamily.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services
            .AddDbContexts(configuration)
            .AddSeeding()
            .ConfigureCustomOptions(configuration)
            .AddProviders()
            .AddIdentity();
        
        return services;
    }
    
    private static void AddIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<User, Role>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<AccountsDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<PermissionManager>();
        services.AddScoped<RolePermissionManager>();
        services.AddScoped<IAccountManager, AccountManager>();
    }

    private static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddTransient<ITokenProvider, JwtTokenProvider>();
        return services;
    }
    
    private static IServiceCollection ConfigureCustomOptions(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));
        services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.ADMIN));
        return services;
    }
    
    private static IServiceCollection AddSeeding(this IServiceCollection services)
    {
        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<AccountsSeederService>();
        return services;
    }
    
    private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AccountsDbContext>(_ =>
            new AccountsDbContext(configuration.GetConnectionString(VolunteerConstant.DATABASE)!));
        
        return services;
    }
    
}