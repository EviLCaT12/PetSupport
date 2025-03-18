using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Commands.UpdatePet;

namespace IntegrationTests.UpdatePet;

public class UpdatePetTests : VolunteerBaseTest
{
    public UpdatePetTests(TestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Update_Pet_Should_Be_Successful()
    {
        var volunteerId = await SeedVolunteerAsync();
        var speciesId = await SeedSpeciesAsync();
        var breedId = await SeedBreedAsync(speciesId);
        var volunteer = WriteContext.Volunteers
            .Include(volunteer => volunteer.AllOwnedPets)
            .ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId);
        var petId = await SeedPetAsync(volunteer!, speciesId, breedId);

        var newPhone = "+7 (111) 111-11-11";
        
        var command = Fixture.ToUpdatePetCommand(volunteerId, petId, speciesId, breedId, newPhone);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdatePetCommand>>();
        
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        
        var phone = ReadContext.Pets.ToList().FirstOrDefault(p => p.OwnerPhone == newPhone);
        phone.Should().NotBeNull();
    }
}