using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteerRequest.Application.Commands.ApproveRequest;
using PetFamily.VolunteerRequest.Domain.Enums;

namespace IntegrationTests.Tests;

public class ApproveRequest : VolunteerRequestBaseTest
{
    public ApproveRequest(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Approve_Request_Should_Be_Successful()
    {
        //Arrange
        Factory.SetupSuccessCreateVolunteerAccount();
        var requestId = await SeedRequest();
        var approveCommand = new ApproveRequestCommand(requestId, "+7 (123) 123-12-12");
        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<ApproveRequestCommand>>();

        //Act
        var result = await sut.HandleAsync(approveCommand, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();

        var request = await ReadDbContext.VolunteerRequests.FirstOrDefaultAsync();
        request!.Status.Should().Be(Status.Approved);
    }
}