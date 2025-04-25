using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using PetFamily.Accounts.Infrastructure.Seeding;
using PetFamily.Discussion.Application.Database;
using PetFamily.Discussion.Infrastructure.Contexts;
using PetFamily.Web;
using Respawn;
using Testcontainers.PostgreSql;

namespace IntegrationTests;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private Respawner _respawner = default!;
    private DbConnection _dbConnection = default!;
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("pet_family_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(cfg =>
        {
            cfg.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ADMIN:USERNAME"] = "admin",
                ["ADMIN:PASSWORD"] = "String123/",
                ["ADMIN:EMAIL"] = "admin@admin.com",
            }!);
        });
        builder.ConfigureTestServices(ConfigureDefaultServices);
    }

    protected virtual void ConfigureDefaultServices(IServiceCollection services)
    {
        var writeDbContext = services.SingleOrDefault(s =>
            s.ServiceType == typeof(WriteDbContext));
        
        var readDbContext = services.SingleOrDefault(s => 
            s.ServiceType == typeof(ReadDbContext));
        
        if(writeDbContext is not null)
            services.Remove(writeDbContext);
        
        if(readDbContext is not null)
            services.Remove(readDbContext);
        

        services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(_dbContainer.GetConnectionString()));
        
        services.AddScoped<IReadDbContext, ReadDbContext>(_ => 
            new ReadDbContext(_dbContainer.GetConnectionString()));

        services.AddDbContext<WriteDbContext>(options =>
        {
            options.EnableServiceProviderCaching(false);
        });


    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        
        var accountSeeder = scope.ServiceProvider.GetRequiredService<AccountsSeeder>();
        await accountSeeder.SeedAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await InitializeRespawnerAsync();
    }
    
    private async Task InitializeRespawnerAsync()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["discussion"] 
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
    
    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}