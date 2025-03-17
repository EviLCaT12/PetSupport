using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Commands.Create;

namespace IntegrationTests.CreateVolunteer;

public class CreateVolunteerTests : VolunteerBaseTest
{
    public CreateVolunteerTests(TestsWebFactory factory) : base(factory)
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