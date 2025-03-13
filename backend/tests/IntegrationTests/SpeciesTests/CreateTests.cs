using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.SpeciesManagement.Commands.Create;

namespace IntegrationTests.SpeciesTests;

public class CreateTests : SpeciesBaseTest
{
    public CreateTests(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Creates_Species_Should_Be_Successful()
    {
        var command = new CreateCommand("Tests");

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateCommand>>();
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var species = ReadContext.Species.First();
        species.Should().NotBeNull();
    }
}