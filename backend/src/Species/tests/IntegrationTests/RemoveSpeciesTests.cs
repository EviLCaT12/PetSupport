using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Species.Application.Commands.Remove;

namespace IntegrationTests;

public class RemoveSpeciesTests : SpeciesBaseTest
{
    public RemoveSpeciesTests(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Remove_Species_Should_Be_Successful()
    {
        var speciesId = await SeedSpeciesAsync();
        
        var command = new RemoveSpeciesCommand(speciesId);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, RemoveSpeciesCommand>>();
        
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(speciesId);

        var species = ReadContext.Species;
        species.Should().BeEmpty();
    }
}