using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Discussion.Application.Commands.Create;
using PetFamily.Discussion.Domain.ValueObjects;

namespace IntegrationTests.Tests;

public class Create : DiscussionBaseTest
{
    public Create(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Create_Discussion_Should_Be_Successful()
    {
        //Arrange
        var member1 = Guid.NewGuid();
        var member2 = Guid.NewGuid();
        List<Guid> members= [];
        members.Add(member1);
        members.Add(member2);
        var command = new CreateCommand(DiscussionsId.NewId().Value, members);
        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateCommand>>();

        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        
        var discussion = WriteDbContext.Discussions
            .ToList()
            .FirstOrDefault(d => d.Id.Value == result.Value);
        discussion.Should().NotBeNull();
    }
}