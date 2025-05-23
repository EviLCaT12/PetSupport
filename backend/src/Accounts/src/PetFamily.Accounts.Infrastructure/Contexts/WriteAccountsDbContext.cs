using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.Accounts.Domain.Entities.AccountEntitites;

namespace PetFamily.Accounts.Infrastructure.Contexts;

public class WriteAccountsDbContext(string connectionString) 
    : IdentityDbContext<User, Role, Guid>
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<AdminAccount> AdminAccounts => Set<AdminAccount>();
    public DbSet<ParticipantAccount> ParticipantAccounts => Set<ParticipantAccount>();
    public DbSet<VolunteerAccount> VolunteerAccounts => Set<VolunteerAccount>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    
    public DbSet<RefreshSession> RefreshSessions => Set<RefreshSession>();
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Role>()
            .ToTable("roles");
         
        builder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("user_claims");
        
        builder.Entity<IdentityUserToken<Guid>>()
            .ToTable("user_tokens");
        
        builder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("user_logins");
        
        builder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("role_claims");
        
        builder.Entity<IdentityUserRole<Guid>>()
            .ToTable("user_roles");
        
        builder.Entity<AdminAccount>()
            .ToTable("admin_accounts");
        
        builder.ApplyConfigurationsFromAssembly(
            typeof(WriteAccountsDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
        
        builder.HasDefaultSchema("account");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create((builder) => {builder.AddConsole();});
}