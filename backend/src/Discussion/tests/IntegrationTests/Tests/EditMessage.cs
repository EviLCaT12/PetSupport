using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Discussion.Application.Commands.ChatMessage;
using PetFamily.Discussion.Application.Commands.DeleteMessage;
using PetFamily.Discussion.Application.Commands.EditMessage;

namespace IntegrationTests.Tests;

public class EditMessageTests : DiscussionBaseTest
{
    public EditMessageTests(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Edit_Message_Should_Be_Successful()
    {
        //Arrange
        var discussion = SeedDiscussionsAsync();
        
        using var scope1 = Factory.Services.CreateScope();
        using var scope2 = Factory.Services.CreateScope();
        
        var chatMessageHandler = scope1.ServiceProvider.GetRequiredService<ICommandHandler<Guid, ChatMessageCommand>>();
        var sut = scope2.ServiceProvider.GetRequiredService<ICommandHandler<EditMessageCommand>>();
        
        var messageId = await chatMessageHandler.HandleAsync(
            new ChatMessageCommand(discussion.Members[0], discussion.Id.Value, "Test"), 
            CancellationToken.None);

        //Act
        var result = await sut.HandleAsync(
            new EditMessageCommand(messageId.Value, discussion.Id.Value,discussion.Members[0], "NewText"), 
            CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();

        var message = WriteDbContext.Messages.ToList().First();
        messageId.Value.Should().Be(message.Id.Value);
        message.Text.Value.Should().Be("NewText");
        message.IsEdited.Should().BeTrue();
        
    }
}