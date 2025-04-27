using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Discussion.Application.Commands.Close;
using PetFamily.Discussion.Domain.Enums;

namespace IntegrationTests.Tests;

public class Close : DiscussionBaseTest
{
    public Close(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Close_Discussion_Should_Be_Successful()
    {
        //Arrange
        var discussion = SeedDiscussionsAsync();
        var command = new CloseCommand(discussion.Id.Value, discussion.Members[0]);
        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<CloseCommand>>();

        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        discussion.Status.Should().Be(Status.Closed);
    }
}