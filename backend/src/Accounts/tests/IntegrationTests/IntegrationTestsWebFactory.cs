using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using PetFamily.Accounts.Infrastructure.Contexts;
using PetFamily.Accounts.Infrastructure.Seeding;
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
            s.ServiceType == typeof(WriteAccountsDbContext));
        
        if(writeDbContext is not null)
            services.Remove(writeDbContext);

        services.AddScoped<WriteAccountsDbContext>(_ =>
            new WriteAccountsDbContext(_dbContainer.GetConnectionString()));
        
        services.AddDbContext<WriteAccountsDbContext>(options =>
        {
            options.EnableServiceProviderCaching(false);
        });
    }
    
    public async Task InitializeAsync() 
    {
        await _dbContainer.StartAsync();
    
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteAccountsDbContext>();
    
        // 1. Сначала создаем БД
        await dbContext.Database.EnsureCreatedAsync();
    
        // 2. Потом подключаем Respawner
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await InitializeRespawnerAsync();
    
        // 3. И только потом заполняем данными
        var seeder = scope.ServiceProvider.GetRequiredService<AccountsSeeder>();
        await seeder.SeedAsync();
    }

    private async Task InitializeRespawnerAsync()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["account"] 
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