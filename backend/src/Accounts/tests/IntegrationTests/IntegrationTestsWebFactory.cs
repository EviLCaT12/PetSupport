using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using NSubstitute;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure;
using PetFamily.Accounts.Infrastructure.Contexts;
using PetFamily.Accounts.Infrastructure.Seeding;
using PetFamily.SharedKernel.Error;
using PetFamily.Web;
using Respawn;
using Testcontainers.PostgreSql;


namespace IntegrationTests;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{ 
    
    private Respawner _respawner = null!;
    private DbConnection _dbConnection = null!;
    private readonly PostgreSqlContainer DbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("pet_family_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
        
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefaultServices);
    }


    protected virtual void ConfigureDefaultServices(IServiceCollection services)
    {
        services
            .RemoveAll(typeof(AccountsDbContext))
            .RemoveAll(typeof(AccountsSeeder));
            
        
        
        services
            .AddScoped<AccountsDbContext>(_ =>
                new AccountsDbContext(DbContainer.GetConnectionString()))
            .AddSingleton<AccountsSeeder>();
    }
    
    public async Task InitializeAsync() 
    {
        await DbContainer.StartAsync();
        
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AccountsDbContext>();
        var seeder = scope.ServiceProvider.GetRequiredService<AccountsSeeder>();
        
        await dbContext.Database.EnsureCreatedAsync();
        DotNetEnv.Env.Load();
        await seeder.SeedAsync();
        
        _dbConnection = new NpgsqlConnection(DbContainer.GetConnectionString());
        await InitializeRespawnerAsync();
    }

    private async Task InitializeRespawnerAsync()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            new RespawnerOptions { DbAdapter = DbAdapter.Postgres});
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);

        using var scope = Services.CreateScope();
        var accountSeeder = scope.ServiceProvider.GetRequiredService<AccountsSeeder>();
        DotNetEnv.Env.Load();
        await accountSeeder.SeedAsync();
    }

    public new async Task DisposeAsync()
    { 
        await DbContainer.StopAsync();
        await DbContainer.DisposeAsync();
    }
}