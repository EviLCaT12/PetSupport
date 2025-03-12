using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.PetManagement.Commands.Create;
using PetFamily.Infrastructure.DbContexts;

namespace IntegrationTests;

public class AddVolunteerTests : IClassFixture<IntegrationTestsWebFactory>
{
    private readonly Fixture _fixture;
    private readonly WriteDbContext _context;
    private readonly IServiceScope _scope;
    private readonly ICommandHandler<Guid, CreateVolunteerCommand> _handler;

    public AddVolunteerTests(IntegrationTestsWebFactory factory)
    {
        _fixture = new Fixture();
        _scope = factory.Services.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        _handler = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
    }

    [Fact]
    public void TestAddVolunteer()
    {
         // Arrange
         
         // Act 
         
         // Assert
    }
}