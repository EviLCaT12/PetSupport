using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Commands.ChangePetHelpStatus;

namespace IntegrationTests.ChangePetStatus;

public class ChangePetHelpStatusTest : VolunteerBaseTest
{
    public ChangePetHelpStatusTest(TestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Change_Pet_Status_Should_Be_Successful()
    {
        // Arrange
        var volunteerId = await SeedVolunteerAsync();
        var volunteer = WriteContext.Volunteers
            .Include(volunteer => volunteer.AllOwnedPets)
            .ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId);
        var petId = await SeedPetAsync(volunteer!);
         
        var command = new ChangePetHelpStatusCommand(volunteerId, petId, "SeekHome");

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<ChangePetHelpStatusCommand>>();
         
        // Act 
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        
        volunteer!.AllOwnedPets.FirstOrDefault()!.HelpStatus.ToString().Should().Be("SeekHome");
    }
    
    [Fact]
    public async Task Change_Pet_Status_With_Incorrect_Statuss_Should_Be_Failed()
    {
        // Arrange
        var volunteerId = await SeedVolunteerAsync();
        var volunteer = WriteContext.Volunteers
            .Include(volunteer => volunteer.AllOwnedPets)
            .ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId);
        var petId = await SeedPetAsync(volunteer!);
         
        var command = new ChangePetHelpStatusCommand(volunteerId, petId, "Incorrect");

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<ChangePetHelpStatusCommand>>();
         
        // Act 
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsFailure.Should().BeTrue();
    }
}