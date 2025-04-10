using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Infrastructure.Contexts;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.VolunteerRequest.Domain.Entities;
using PetFamily.VolunteerRequest.Domain.ValueObjects;
using PetFamily.VolunteerRequest.Infrastructure.DbContexts;

namespace IntegrationTests;

public class BaseTest
{
    protected readonly WriteContext WriteContext;
    protected readonly IServiceScope Scope;
    protected readonly IntegrationTestsWebFactory Factory;
    
    protected BaseTest(IntegrationTestsWebFactory factory)
    {
        Factory = factory;
        Scope = factory.Services.CreateScope();
        
        WriteContext = Scope.ServiceProvider.GetRequiredService<WriteContext>();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        Scope.Dispose();
        await Factory.ResetDatabaseAsync();
    }

    public VolunteerInfo CreateValidVolunteerInfo()
    {
        var fullName = Fio.Create("string", "string", "string").Value;
        
        var description = Description.Create(Guid.NewGuid().ToString()).Value;
        
        var email = Email.Create(Guid.NewGuid() + "@mail.ru").Value;
        
        var exp = YearsOfExperience.Create(10).Value;

        return new VolunteerInfo(
            fullName,
            description,
            email,
            exp);
    }

    public VolunteerRequest CreateValidVolunteerRequest()
    {
        var id = VolunteerRequestId.NewVolunteerRequestId();
        
        var userId = Guid.NewGuid();

        var volunteerInfo = CreateValidVolunteerInfo();
        
        return new VolunteerRequest(id, userId, volunteerInfo);
    }
    
}