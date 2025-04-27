using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussion.Application.Database;
using PetFamily.Discussion.Domain.Entities;
using PetFamily.Discussion.Domain.ValueObjects;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Discussion.Application.Commands.ChatMessage;

public class ChatMessageHandler : ICommandHandler<Guid, ChatMessageCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChatMessageHandler> _logger;
    private readonly IValidator<ChatMessageCommand> _validator;
    private readonly IMessageRepository _messageRepository;
    private readonly IDiscussionRepository _discussionRepository;

    public ChatMessageHandler(
        [FromKeyedServices(ModuleKey.Discussion)] IUnitOfWork unitOfWork,
        ILogger<ChatMessageHandler> logger,
        IValidator<ChatMessageCommand> validator,
        IMessageRepository messageRepository,
        IDiscussionRepository discussionRepository)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = validator;
        _messageRepository = messageRepository;
        _discussionRepository = discussionRepository;
    }
    public async Task<Result<Guid, ErrorList>> HandleAsync(ChatMessageCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var discussionId = DiscussionsId.Create(command.DiscussionId).Value;
        var discussion = await _discussionRepository.GetByIdAsync(discussionId, cancellationToken);
        if (discussion is null)
        {
            _logger.LogWarning($"Discussion with id {discussionId} not found");
            return Errors.General.ValueNotFound(discussionId.Value).ToErrorList();
        }
        
        var isUserInDiscussion = discussion.IsUserInDiscussion(command.UserId);
        if (isUserInDiscussion.IsFailure)
            return isUserInDiscussion.Error.ToErrorList();

        var messageId = MessageId.NewId();
        var text = Text.Create(command.Text).Value;
        var message = new Message(
            messageId,
            command.UserId,
            text);
        discussion.AddComment(message);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return messageId.Value;
    }
}