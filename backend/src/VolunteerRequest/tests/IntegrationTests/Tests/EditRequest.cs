using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteerRequest.Application.Commands.EditRequest;
using PetFamily.VolunteerRequest.Domain.Enums;

namespace IntegrationTests.Tests;

public class EditRequest : VolunteerRequestBaseTest
{
    public EditRequest(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Edit_Request_Should_Be_Successful()
    {
        //Arrange
        var requestId = await SeedRequest();
        var oldRequest = WriteDbContext.VolunteerRequests.ToList().FirstOrDefault();
        await ChangeStatusForRequest(oldRequest!, Status.Submitted);
        var editCommand = Fixture.AddEditCommand(requestId);
        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<EditCommand>>();
        
        //Act
        var result = await sut.HandleAsync(editCommand, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        
        var newRequest = await ReadDbContext.VolunteerRequests.FirstOrDefaultAsync();
        newRequest!.Id.Should().Be(oldRequest!.Id.Value);
        newRequest.Email.Should().NotBeEquivalentTo("email@mail.ru");
        newRequest.Status.Should().Be(Status.Submitted);
    }
}