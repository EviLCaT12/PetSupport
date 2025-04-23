using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.VolunteerRequest.Application.Commands.TakeRequestOnReview;
using PetFamily.VolunteerRequest.Domain.Enums;

namespace IntegrationTests.Tests;

public class TakeRequestOnReview : VolunteerRequestBaseTest
{
    public TakeRequestOnReview(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Take_Request_On_Review_Should_Be_Successful()
    {
        //Arrange
        Factory.SetupSuccessCreateDiscussionForVolunteerRequest();
        var requestId = await SeedRequest();
        var reviewCommand = new TakeRequestOnReviewCommand(requestId, Guid.NewGuid());
        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<TakeRequestOnReviewCommand>>();

        //Act
        var result = await sut.HandleAsync(reviewCommand, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();

        var request = ReadDbContext.VolunteerRequests.First();
        request.Status.Should().Be(Status.OnReview);
        request.AdminId.Should().NotBeNull();
        request.DiscussionId.Should().NotBeNull();
    }
}