using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.VolunteerRequest.Application.Queries.GetAllSubmittedRequestsWithPagination;
using PetFamily.VolunteerRequest.Contracts.Dto;
using PetFamily.VolunteerRequest.Domain.Entities;

namespace IntegrationTests.Tests;

public class GetAllSubmittedRequest : VolunteerRequestBaseTest
{
    public GetAllSubmittedRequest(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_All_Submitted_Request_Should_Be_Successful()
    {
        //Arrange
        for (var i = 0; i <= 10; i++)
        {
            await SeedRequest();
        }
        var getAllQuery = new GetAllSubmittedRequestQuery(1, 10);
        var sut = Scope.ServiceProvider
            .GetRequiredService<IQueryHandler<PagedList<VolunteerRequestDto>, GetAllSubmittedRequestQuery>>();

        //Act
        var result = await sut.HandleAsync(getAllQuery, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        
        var requests = result.Value.Items;
        requests.Count.Should().Be(10);
    }
}