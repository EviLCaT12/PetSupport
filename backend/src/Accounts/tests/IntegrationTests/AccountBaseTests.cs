using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Infrastructure;
using PetFamily.Accounts.Infrastructure.Contexts;

namespace IntegrationTests;

public class AccountBaseTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly Fixture Fixture;
    protected readonly WriteAccountsDbContext WriteContext;
    protected readonly IServiceScope Scope;
    protected readonly IntegrationTestsWebFactory Factory;
    
    public AccountBaseTests(IntegrationTestsWebFactory factory)
    {
        Factory = factory;
        Scope = factory.Services.CreateScope();
        
        WriteContext = Scope.ServiceProvider.GetRequiredService<WriteAccountsDbContext>();
        Fixture = new Fixture();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    async Task IAsyncLifetime.DisposeAsync()
    {
        await Factory.ResetDatabaseAsync();
        Scope.Dispose();
    }
}