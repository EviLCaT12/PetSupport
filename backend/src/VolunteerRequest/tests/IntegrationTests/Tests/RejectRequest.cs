using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteerRequest.Application.Commands.RejectRequest;
using PetFamily.VolunteerRequest.Domain.Enums;

namespace IntegrationTests.Tests;

public class RejectRequest : VolunteerRequestBaseTest
{
    public RejectRequest(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Reject_Request_Should_Be_Successful()
    {
        //Arrange
        Factory.SetupSuccessBanUserToSendVolunteerRequest();
        var requestId = await SeedRequest();
        var rejectCommand = new RejectRequestCommand(requestId, Guid.NewGuid().ToString());
        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<RejectRequestCommand>>();
        
        //Act
        var result = await sut.HandleAsync(rejectCommand, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();

        var request = await ReadDbContext.VolunteerRequests.FirstAsync();
        request.Status.Should().Be(Status.Rejected);
        request.Rejected_Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(100));
        request.RejectionComment.Should().NotBeEmpty();
    }
}