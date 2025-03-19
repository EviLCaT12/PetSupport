using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Infrastructure;

namespace IntegrationTests;

public class AccountBaseTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly Fixture Fixture;
    protected readonly AuthorizationDbContext WriteContext;
    protected readonly IServiceScope Scope;
    protected readonly IntegrationTestsWebFactory Factory;
    
    protected AccountBaseTests(IntegrationTestsWebFactory factory)
    {
        Factory = factory;
        Scope = factory.Services.CreateScope();
        WriteContext = Scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
        Fixture = new Fixture();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        Scope.Dispose();
        await Factory.ResetDatabaseAsync();
    }
}