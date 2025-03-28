using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Application.Commands.UpdateMainInfo;

namespace IntegrationTests.UpdateMainInfo;

public class UpdateMainInfoTests : VolunteerBaseTest
{
    public UpdateMainInfoTests(TestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Create_Volunteer_Should_Be_Successful()
    {
        //Arrange
        var volunteerId = await SeedVolunteerAsync();
        var newPhone = "+7 (111) 111-11-11";
        var command = Fixture.ToUpdateVolunteerMainInfoCommand(volunteerId, newPhone);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateVolunteerMainInfoCommand>>();
        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        
        var volunteer = await ReadContext.Volunteers.FirstOrDefaultAsync(v => v.Id == volunteerId);
        volunteer!.Phone.Should().Be(newPhone);
    }
}