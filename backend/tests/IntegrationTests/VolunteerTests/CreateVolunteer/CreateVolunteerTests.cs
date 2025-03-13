using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.PetManagement.Commands.Create;

namespace IntegrationTests.VolunteerTests.CreateVolunteer;

public class CreateVolunteerTests : VolunteerBaseTest
{
    public CreateVolunteerTests(VolunteerTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Create_Volunteer_Should_Be_Successful()
    {
        //Arrange
        var command = Fixture.ToCreateVolunteerCommand();

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }
}