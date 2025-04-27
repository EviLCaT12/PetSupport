using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteerRequest.Application.Commands.Create;

namespace IntegrationTests.Tests;

public class Create : VolunteerRequestBaseTest
{
    public Create(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Create_Volunteer_Request_Should_be_Successful()
    {
        //Arrange
        Factory.SetupSuccessIsUserAlreadyVolunteer();
        Factory.SetupSuccessIsUserCanSendVolunteerRequests();
        var command = Fixture.AddCreateCommand();
        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateVolunteerRequestCommand>>();

        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        
        var request = WriteDbContext.VolunteerRequests
            .ToList()
            .FirstOrDefault(r => r.Id.Value == result.Value);
        request.Should().NotBeNull();
    }
}