using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto.Shared;
using PetFamily.VolunteerRequest.Application.Commands.Create;

namespace IntegrationTests;

public class CreateTests : BaseTest
{
    public CreateTests(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Create_Volunteer_Request_Should_Be_Successful()
    {
        //Arrange
        var command = new CreateVolunteerRequestCommand(
            Guid.NewGuid(),
            new FioDto("string", "string", "string"),
            Guid.NewGuid().ToString(),
            Guid.NewGuid() + "@mail.ru",
            10);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateVolunteerRequestCommand>>();
        
        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        
        var volunteerRequest = await WriteContext.VolunteerRequests.FirstAsync(CancellationToken.None);
        volunteerRequest.Should().NotBeNull();
    }
    
}