using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Commands.ChangePetPosition;

namespace IntegrationTests.ChangePetPosition;

public class ChangePetPositionTests : VolunteerBaseTest
{
    public ChangePetPositionTests(TestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Change_Pet_Position_Should_Be_Successful()
    {
        var volunteerId = await SeedVolunteerAsync();
        var volunteer = WriteContext.Volunteers
            .ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId);
        var petId1 = await SeedPetAsync(volunteer!);
        var petId2 = await SeedPetAsync(volunteer!);
        var petId3 = await SeedPetAsync(volunteer!);
        
        var command = new ChangePetPositionCommand(
            volunteerId,
            petId3,
            1);
        
        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<ChangePetPositionCommand>>();
        
        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        var pet = ReadContext.Pets.FirstOrDefault(p => p.Id == petId3);
        pet!.Position.Should().Be(1);
    }
}