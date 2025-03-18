using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Commands.DeletePet;

namespace IntegrationTests.DeletePet;

public class DeletePetTests : VolunteerBaseTest
{
    public DeletePetTests(TestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Soft_Delete_Pet_Should_Be_Successful()
    {
        //Arange
        var volunteerId = await SeedVolunteerAsync();
        var volunteer = WriteContext.Volunteers
            .Include(v => v.AllOwnedPets)
            .ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId);
        var petId = await SeedPetAsync(volunteer!);
        
        var command = new DeletePetCommand(volunteerId, petId);

        var sut = Scope.ServiceProvider
            .GetServices<ICommandHandler<DeletePetCommand>>()
            .FirstOrDefault(x => x.GetType() == typeof(SoftDeletePetHandler));
        
        //Act
        var result = await sut!.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();

        var petInDb = WriteContext.Volunteers
            .Include(v => v.AllOwnedPets)
            .ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId)!
            .AllOwnedPets
            .FirstOrDefault(p => p.Id.Value == petId);
        
        petInDb!.IsDeleted.Should().BeTrue();
        petInDb.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Hard_Delete_Pet_Should_Be_Successful()
    {
        //Arange
        var volunteerId = await SeedVolunteerAsync();
        var volunteer = WriteContext.Volunteers
            .Include(v => v.AllOwnedPets)
            .ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId);
        var petId = await SeedPetAsync(volunteer!);
        
        var command = new DeletePetCommand(volunteerId, petId);

        var sut = Scope.ServiceProvider
            .GetServices<ICommandHandler<DeletePetCommand>>()
            .FirstOrDefault(x => x.GetType() == typeof(HardDeletePetHandler));
        
        //Act
        var result = await sut!.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        
        var petInDb = ReadContext.Pets.FirstOrDefault(p => p.Id == petId);
        petInDb.Should().BeNull();
    }
}