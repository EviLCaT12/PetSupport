using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.DataBase;

namespace IntegrationTests;

public class AddVolunteerTests : IClassFixture<IntegrationTestsWebFactory>
{
    private readonly IntegrationTestsWebFactory _factory;

    public AddVolunteerTests(IntegrationTestsWebFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public void TestAddVolunteer()
    {
        var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        var volunteers = dbContext.Volunteers.ToList();
        
        volunteers.Should().BeEmpty();
    }
}