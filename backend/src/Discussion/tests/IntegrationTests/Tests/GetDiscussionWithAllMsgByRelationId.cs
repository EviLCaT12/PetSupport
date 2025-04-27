using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;
using PetFamily.Discussion.Application.Commands.ChatMessage;
using PetFamily.Discussion.Application.Queries.GetDiscussionWIthAllMsgByRelationId;
using PetFamily.Discussion.Domain.Entities;
using PetFamily.Discussion.Domain.ValueObjects;

namespace IntegrationTests.Tests;

public class GetDiscussionWithAllMsgByRelationId : DiscussionBaseTest
{
    public GetDiscussionWithAllMsgByRelationId(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_Discussion_With_All_Msg_By_Relation_Id_Should_Be_Successful()
    {
        //Arrange
        var relationId = Guid.NewGuid();
        var member1 = Guid.NewGuid();
        var member2 = Guid.NewGuid();
        List<Guid> members = [member1, member2];
        var discussion = Discussion.Create(
            DiscussionsId.NewId(),
            relationId,
            members).Value;
        
        var message1 = new Message(
            MessageId.NewId(),
            member1,
            Text.Create("test1").Value);
        var message2 = new Message(
            MessageId.NewId(),
            member1,
            Text.Create("test2").Value);
        var message3 = new Message(
            MessageId.NewId(),
            member1,
            Text.Create("test3").Value);
        List<Message> messages = [message1, message2, message3];
        foreach (var message in messages)
        {
            discussion.AddComment(message);
        }
        
        WriteDbContext.Discussions.Add(discussion);
        WriteDbContext.SaveChanges();

        var query = new GetDiscussionWithAllMsgByRelationIdQuery(relationId);
        var sut = Scope.ServiceProvider.GetRequiredService<GetDiscussionWithAllMsgByRelationIdHandler>();

        //Act
        var result = await sut.HandleAsync(query, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Messages.Should().HaveCount(3);

    }
}