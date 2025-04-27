using System.Transactions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Discussion.Application.Commands.ChatMessage;
using PetFamily.Discussion.Application.Commands.DeleteMessage;
using PetFamily.Discussion.Domain.Entities;
using PetFamily.Discussion.Domain.ValueObjects;
using PetFamily.Discussion.Infrastructure.Contexts;

namespace IntegrationTests.Tests;

public class DeleteMessage : DiscussionBaseTest
{
    public DeleteMessage(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Delete_Message_Should_Be_Successful()
    {
        // Arrange
        var member1 = Guid.NewGuid();
        var member2 = Guid.NewGuid();
        var members = new List<Guid> { member1, member2 };
    
        var discussion = Discussion.Create(
            DiscussionsId.NewId(),
            Guid.NewGuid(),
            members).Value;
        
        WriteDbContext.Discussions.Add(discussion);
        await WriteDbContext.SaveChangesAsync();
        
        using var scope1 = Factory.Services.CreateScope();
        using var scope2 = Factory.Services.CreateScope();
        
        var chatMessageHandler = scope1.ServiceProvider.GetRequiredService<ICommandHandler<Guid, ChatMessageCommand>>();
        var sut = scope2.ServiceProvider.GetRequiredService<ICommandHandler<DeleteMessageCommand>>();
        
        var messageId = await chatMessageHandler.HandleAsync(
            new ChatMessageCommand(discussion.Members[0], discussion.Id.Value, "Test"), 
            CancellationToken.None);

        //Act
        var result = await sut.HandleAsync(
            new DeleteMessageCommand(discussion.Members[0], discussion.Id.Value, messageId.Value), 
            CancellationToken.None);
    
        // Assert
        result.IsSuccess.Should().BeTrue();
        WriteDbContext.Messages
            .ToList()
            .FirstOrDefault(m => m.Id.Value == messageId.Value)
            .Should().BeNull();
    }
}