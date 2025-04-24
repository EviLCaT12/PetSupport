using System.Data.Common;
using Contracts;
using Contracts.Requests;
using CSharpFunctionalExtensions;
using DotNetEnv;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using NSubstitute;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Accounts.Infrastructure.Seeding;
using PetFamily.SharedKernel.Error;
using PetFamily.VolunteerRequest.Application.Abstractions;
using PetFamily.VolunteerRequest.Infrastructure.DbContexts;
using PetFamily.Web;
using Respawn;
using Testcontainers.PostgreSql;

namespace IntegrationTests;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IAccountContract _accountContractMock = Substitute.For<IAccountContract>();
    private readonly IDiscussionContract _discussionContractMock = Substitute.For<IDiscussionContract>();
    private  Respawner _respawner = default!;
    private  DbConnection _dbConnection = default!;
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("pet_family_tests")
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
            s.ServiceType == typeof(WriteContext));
        
        var readDbContext = services.SingleOrDefault(s =>
            s.ServiceType == typeof(IReadDbContext));

        var accountService = services.SingleOrDefault(s =>
            s.ServiceType == typeof(IAccountContract));
        
        var discussionService = services.SingleOrDefault(s =>
            s.ServiceType == typeof(IDiscussionContract));
        
        if(writeDbContext is not null)
            services.Remove(writeDbContext);
        
        if(readDbContext is not null)
            services.Remove(readDbContext);
        
        if (accountService is not null)
            services.Remove(accountService);
        
        if (discussionService is not null)
            services.Remove(discussionService);

        services.AddScoped<WriteContext>(_ =>
            new WriteContext(_dbContainer.GetConnectionString()));
        
        services.AddScoped<IReadDbContext, ReadContext>(_ =>
            new ReadContext(_dbContainer.GetConnectionString()));
        
        services.AddScoped<IAccountContract>(_ => _accountContractMock);
        
        services.AddScoped<IDiscussionContract>(_ => _discussionContractMock);
    }

    public void SetupSuccessCreateDiscussionForVolunteerRequest()
    {
        _discussionContractMock
            .CreateDiscussionForVolunteerRequest(Arg.Any<CreateDiscussionRequest>(), Arg.Any<CancellationToken>())
            .Returns(Guid.NewGuid());
    }
    
    public void SetupSuccessIsUserAlreadyVolunteer()
    {
        _accountContractMock
            .IsUserAlreadyVolunteer(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(false);
    }

    public void SetupSuccessIsUserCanSendVolunteerRequests()
    {
        _accountContractMock
            .IsUserCanSendVolunteerRequests(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(true);
    }
    
    public void SetupSuccessCreateVolunteerAccount()
    {
        _accountContractMock
            .CreateVolunteerAccount(Arg.Any<ApproveRequestRequest>(), Arg.Any<CancellationToken>())
            .Returns(Guid.NewGuid());
    }

    public void SetupSuccessBanUserToSendVolunteerRequest()
    {
        _accountContractMock
            .BanUserToSendVolunteerRequest(Arg.Any<BanUserToSendVolunteerRequestRequest>(),
                Arg.Any<CancellationToken>())
            .Returns(UnitResult.Success<ErrorList>());
    }
    
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        using var scope = Services.CreateScope();
        var writeDbContext = scope.ServiceProvider.GetRequiredService<WriteContext>();
        await writeDbContext.Database.EnsureCreatedAsync();

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
            SchemasToInclude = ["account", "volunteer_request"] //FIXME: Добавить позже discussion 
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