using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.AddPet;

namespace IntegrationTests.VolunteerTests.AddPetTests;

public class AddPetTests : VolunteerBaseTest
{
    public AddPetTests(VolunteerTestsWebFactory factory) : base(factory)
    { }
    
    [Fact]
    public async Task Add_pet_to_database_should_work()
    {
         // Arrange
         var volunteerId = await SeedVolunteerAsync();
         var speciesId = await SeedSpeciesAsync();
         var species = WriteContext.Species.ToList().FirstOrDefault(s => s.Id.Value == speciesId);
         var breedId = await SeedBreedAsync(species!);
         
         var command = Fixture.ToAddPetCommand(
             volunteerId,
             speciesId,
             breedId);

         var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddPetCommand>>();
         
         // Act 
         var result = await sut.HandleAsync(command, CancellationToken.None);
         
         // Assert
         result.IsSuccess.Should().BeTrue();
         result.Value.Should().NotBeEmpty();
         
         var pet = await ReadContext.Pets.FirstOrDefaultAsync();
         
         pet.Should().NotBeNull();
         pet.VolunteerId.Should().Be(volunteerId);
    }
}