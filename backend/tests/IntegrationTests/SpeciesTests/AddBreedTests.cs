using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.SpeciesManagement.Commands.AddBreeds;

namespace IntegrationTests.SpeciesTests;

public class AddBreedTests : SpeciesBaseTest
{
    public AddBreedTests(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Add_Breed_Should_Be_Successful()
    {
        var speciesId = await SeedSpeciesAsync();

        var command = new AddBreedsCommand(speciesId, ["tests"]);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<List<Guid>, AddBreedsCommand>>();
        
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        
        var breed = ReadContext.Breeds.Single(b => b.Id == result.Value.First());
        breed.Should().NotBeNull();
    }
}