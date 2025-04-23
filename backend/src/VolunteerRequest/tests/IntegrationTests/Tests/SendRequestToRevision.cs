using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteerRequest.Application.Commands.SendRequestToRevision;
using PetFamily.VolunteerRequest.Domain.Enums;

namespace IntegrationTests.Tests;

public class SendRequestToRevision : VolunteerRequestBaseTest
{
    public SendRequestToRevision(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Send_Request_To_Revision_Should_Be_Successful()
    {
        //Arrange
        var requestId = await SeedRequest();
        var revisionCommand = new SendRequestToRevisionCommand(requestId, Guid.NewGuid().ToString());
        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<SendRequestToRevisionCommand>>();

        //Act
        var result = await sut.HandleAsync(revisionCommand, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();

        var request = ReadDbContext.VolunteerRequests.First();
        request.Status.Should().Be(Status.RevisionRequired);
        request.RejectionComment.Should().NotBeEmpty();
    }
}