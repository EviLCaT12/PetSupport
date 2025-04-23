using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.VolunteerRequest.Application.Queries.GetRequestsForCurrentAdmin;
using PetFamily.VolunteerRequest.Contracts.Dto;

namespace IntegrationTests.Tests;

public class GetRequestsForCurrentAdmin : VolunteerRequestBaseTest
{
    public GetRequestsForCurrentAdmin(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_Requests_For_Current_Admin_Should_Be_Successful()
    {
        //Arrange
        var adminId = Guid.NewGuid();
        for (var i = 0; i <= 10; i++)
        {
            await SeedRequest();
        }

        var requests = WriteDbContext.VolunteerRequests.ToList();
        foreach (var request in requests)
        {
            request.TakeRequestOnReview(adminId, Guid.NewGuid());
            await WriteDbContext.SaveChangesAsync();
        }
        
        var getForCurrentAdminQuery = new GetRequestsForCurrentAdminQuery(adminId, "OnReview", 1, 10);

        var sut = Scope.ServiceProvider
            .GetRequiredService<IQueryHandler<PagedList<VolunteerRequestDto>, GetRequestsForCurrentAdminQuery>>();
        //Act
        var result = await sut.HandleAsync(getForCurrentAdminQuery, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();

        var requestList = result.Value.Items;
        requestList.Should().HaveCount(10);
    }
}