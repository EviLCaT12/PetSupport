using System.Transactions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Commands.AddPet;

namespace IntegrationTests.AddPetTests;

public class AddPetTests : VolunteerBaseTest
{
    public AddPetTests(TestsWebFactory factory) : base(factory)
    { }
    
    [Fact]
    public async Task Add_pet_to_database_should_work()
    {
        // Arrange

        var volunteerId = await SeedVolunteerAsync();
        var speciesId = await SeedSpeciesAsync();
        var breedId = await SeedBreedAsync(speciesId);

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

        var pet = ReadContext.Pets.ToList().First();

        pet.Should().NotBeNull();
        pet.VolunteerId.Should().Be(volunteerId);
    }
}