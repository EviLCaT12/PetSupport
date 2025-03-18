using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Species.Application.Commands.RemoveBreed;

namespace IntegrationTests;

public class RemoveBreedTests : SpeciesBaseTest
{
    public RemoveBreedTests(IntegrationTestsWebFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task Remove_Breed_Should_Be_Successful()
    {
        var speciesId = await SeedSpeciesAsync();
        var species = WriteContext.Species.ToList().FirstOrDefault(s => s.Id.Value == speciesId);
        var breedId = await SeedBreedAsync(species!);
        
        var command = new RemoveBreedCommand(speciesId, breedId);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, RemoveBreedCommand>>();
        
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(breedId);

        var breed = ReadContext.Breeds;
        breed.Should().BeEmpty();
    }
    
}