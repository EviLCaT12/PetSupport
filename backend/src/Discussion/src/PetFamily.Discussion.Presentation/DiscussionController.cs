using Contracts.Dtos;
using Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Abstractions;
using PetFamily.Discussion.Application.Commands.ChatMessage;
using PetFamily.Discussion.Application.Commands.Close;
using PetFamily.Discussion.Application.Commands.DeleteMessage;
using PetFamily.Discussion.Application.Commands.EditMessage;
using PetFamily.Discussion.Application.Queries.GetDiscussionWIthAllMsgByRelationId;
using PetFamily.Framework;
using PetFamily.Framework.Authorization;

namespace PetFamily.Discussion.Presentation;

public class DiscussionController : ApplicationController
{
    [Permission(Permissions.Discussions.CloseDiscussion)]
    [HttpPost("{userId:guid}/{discussionId:guid}/close")]
    public async Task<ActionResult> CloseDiscussion(
        [FromRoute] Guid userId, Guid discussionId,
        [FromServices] ICommandHandler<CloseCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new CloseCommand(discussionId, userId);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }

    [Permission(Permissions.Discussions.ChatMessage)]
    [HttpPost("{userId:guid}/{discussionId:guid}/message")]
    public async Task<ActionResult<Guid>> CloseDiscussion(
        [FromRoute] Guid userId, Guid discussionId,
        [FromBody] ChatMessageRequest request,
        [FromServices] ICommandHandler<Guid, ChatMessageCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new ChatMessageCommand(userId, discussionId, request.Text);
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [Permission(Permissions.Discussions.EditMessage)]
    [HttpPut("{userId:guid}/{discussionId:guid}/{messageId:guid}/edit")]
    public async Task<ActionResult> EditMessage(
        [FromRoute] Guid userId, Guid discussionId, Guid messageId,
        [FromBody] ChatMessageRequest request,
        [FromServices] ICommandHandler<EditMessageCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new EditMessageCommand(messageId, discussionId, userId, request.Text);
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
    
    [Permission(Permissions.Discussions.DeleteMessage)]
    [HttpDelete("{userId:guid}/{discussionId:guid}/{messageId:guid}/remove")]
    public async Task<ActionResult> DeleteMessage(
        [FromRoute] Guid userId, Guid discussionId, Guid messageId,
        [FromServices] ICommandHandler<DeleteMessageCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteMessageCommand(userId, discussionId, messageId);
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }

    [Permission(Permissions.Discussions.GetDiscussion)]
    [HttpGet("{discussionId:guid}")]
    public async Task<ActionResult> GetDiscussionWithAllMsgByRelationId(
        [FromRoute] Guid discussionId,
        [FromServices] IQueryHandler<DiscussionDto, GetDiscussionWithAllMsgByRelationIdQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetDiscussionWithAllMsgByRelationIdQuery(discussionId);
        
        var result = await handler.HandleAsync(query, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}