using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Discussion.Application.Commands.ChatMessage;


namespace IntegrationTests.Tests;

public class ChatMessage : DiscussionBaseTest
{
    public ChatMessage(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Chat_Message_Should_Successful()
    {
        //Arrange
        var discussion = SeedDiscussionsAsync();

        var command = new ChatMessageCommand(
            discussion.Members[0],
            discussion.Id.Value,
            Guid.NewGuid().ToString());

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, ChatMessageCommand>>();
        
        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        
        var message = WriteDbContext.Messages
            .ToList()
            .FirstOrDefault(m => m.Id.Value == result.Value);
        
        message.Should().NotBeNull();
    }
}