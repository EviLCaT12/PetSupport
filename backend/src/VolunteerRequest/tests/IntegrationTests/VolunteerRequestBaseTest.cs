using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.VolunteerRequest.Application.Abstractions;
using PetFamily.VolunteerRequest.Domain.Entities;
using PetFamily.VolunteerRequest.Domain.Enums;
using PetFamily.VolunteerRequest.Domain.ValueObjects;
using PetFamily.VolunteerRequest.Infrastructure.DbContexts;

namespace IntegrationTests;

public class VolunteerRequestBaseTest : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestsWebFactory Factory;
    protected readonly IServiceScope Scope;
    protected readonly WriteContext WriteDbContext;
    protected readonly IReadDbContext ReadDbContext;
    protected readonly Fixture Fixture;

    public VolunteerRequestBaseTest(IntegrationTestsWebFactory factory)
    {
        Factory = factory;
        Scope = factory.Services.CreateScope();
        WriteDbContext = Scope.ServiceProvider.GetRequiredService<WriteContext>();
        ReadDbContext = Scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        Fixture = new Fixture();
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    async Task IAsyncLifetime.DisposeAsync()
    {
        await Factory.ResetDatabaseAsync();
        Scope.Dispose();
    }

    public async Task<Guid> SeedRequest(Guid? userId = null)
    {
        var id = VolunteerRequestId.NewVolunteerRequestId();
            
        var fullName = Fio.Create(
            "string",
            "string",
            "string").Value;

        var description = Description.Create(Guid.NewGuid().ToString()).Value;
            
        var email = Email.Create("email@mail.ru").Value;
            
        var experience = YearsOfExperience.Create(1).Value;
            
        var volunteerInfo = new VolunteerInfo(fullName, description, email, experience);
            
        var volunteerRequest = new VolunteerRequest(id, userId ?? Guid.NewGuid(), volunteerInfo);

        await WriteDbContext.VolunteerRequests.AddAsync(volunteerRequest);
        await WriteDbContext.SaveChangesAsync();

        return id.Value;
    }

    public async Task ChangeStatusForRequest(VolunteerRequest request, Status statusToChange)
    {
        switch (statusToChange)
        {
            case Status.Submitted:
            {
                var comment = new RejectionComment(Description.Create(Guid.NewGuid().ToString()).Value);
                request.SendForRevision(comment);
                break;
            }
        }
        await WriteDbContext.SaveChangesAsync();
    }
}