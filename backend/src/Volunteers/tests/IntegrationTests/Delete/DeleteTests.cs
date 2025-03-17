using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Commands.Delete;

namespace IntegrationTests.Delete;

public class DeleteTests : VolunteerBaseTest
{
    public DeleteTests(TestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Soft_Delete_Should_Be_Successful()
    {
        var volunteerId = await SeedVolunteerAsync();
        var volunteer = WriteContext.Volunteers
            .ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId);
        var petId = await SeedPetAsync(volunteer!);
        
        var command = new DeleteVolunteerCommand(volunteerId);

        var sut = Scope.ServiceProvider
            .GetServices<ICommandHandler<Guid, DeleteVolunteerCommand>>()
            .FirstOrDefault(s => s.GetType() == typeof(SoftDeleteVolunteerHandler));
        
        var result = await sut!.HandleAsync(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task Hard_Delete_Should_Be_Successful()
    {
        var volunteerId = await SeedVolunteerAsync();
        var volunteer = WriteContext.Volunteers
            .ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId);
        var petId = await SeedPetAsync(volunteer!);
        
        var command = new DeleteVolunteerCommand(volunteerId);

        var sut = Scope.ServiceProvider
            .GetServices<ICommandHandler<Guid, DeleteVolunteerCommand>>()
            .FirstOrDefault(s => s.GetType() == typeof(HardDeleteVolunteerHandler));
        
        var result = await sut!.HandleAsync(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }
}