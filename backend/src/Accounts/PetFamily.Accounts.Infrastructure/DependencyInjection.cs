using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Accounts.Infrastructure.TokenProviders;
using PetFamily.SharedKernel.Constants;

namespace PetFamily.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddTransient<ITokenProvider, JwtTokenProvider>();
        
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));
        
        services.AddScoped<AuthorizationDbContext>(_ =>
            new AuthorizationDbContext(configuration.GetConnectionString(VolunteerConstant.DATABASE)!));
        
        services
            .AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AuthorizationDbContext>();
        
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtTokens = configuration.GetSection(JwtOptions.JWT).Get<JwtOptions>()
                    ?? throw new ApplicationException("Missing JWT configuration");
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtTokens.Issuer,
                    ValidAudience = jwtTokens.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokens.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false
                };
            });
        
        return services;
    }
}