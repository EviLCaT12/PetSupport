using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.VolunteerRequest.Application.Queries.GetRequestsForCurrentUser;
using PetFamily.VolunteerRequest.Contracts.Dto;

namespace IntegrationTests.Tests;

public class GetRequestsForCurrentUser : VolunteerRequestBaseTest
{
    public GetRequestsForCurrentUser(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_Requests_For_Current_User_Should_Be_Successful()
    {
        //Arrange
        var userId = Guid.NewGuid();
        for (var i = 0; i <= 10; i++)
        {
            await SeedRequest(userId);
        }
        var getRequestsForCurrentUser = new GetRequestsForCurrentUserQuery(userId, "Submitted", 1, 10);
        var sut = Scope.ServiceProvider
            .GetRequiredService<IQueryHandler<PagedList<VolunteerRequestDto>, GetRequestsForCurrentUserQuery>>();
        //Act
        var result = await sut.HandleAsync(getRequestsForCurrentUser, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();

        var requests = result.Value.Items;
        requests.Should().HaveCount(10);
    }
}