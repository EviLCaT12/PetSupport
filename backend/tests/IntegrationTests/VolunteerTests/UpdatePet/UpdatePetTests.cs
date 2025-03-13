using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.UpdatePet;

namespace IntegrationTests.VolunteerTests.UpdatePet;

public class UpdatePetTests : VolunteerBaseTest
{
    public UpdatePetTests(VolunteerTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Update_Pet_Should_Be_Successful()
    {
        var volunteerId = await SeedVolunteerAsync();
        var speciesId = await SeedSpeciesAsync();
        var species = WriteContext.Species.ToList().FirstOrDefault(s => s.Id.Value == speciesId);
        var breedId = await SeedBreedAsync(species!);
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
        
        var phone = await ReadContext.Pets.FirstOrDefaultAsync(p => p.OwnerPhone == newPhone);
        phone.Should().NotBeNull();
    }
}