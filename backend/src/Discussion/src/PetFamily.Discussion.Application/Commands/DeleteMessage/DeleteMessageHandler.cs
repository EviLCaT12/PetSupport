using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussion.Application.Database;
using PetFamily.Discussion.Domain.ValueObjects;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Discussion.Application.Commands.DeleteMessage;

public class DeleteMessageHandler : ICommandHandler<DeleteMessageCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteMessageCommand> _validator;
    private readonly ILogger<DeleteMessageHandler> _logger;
    private readonly IMessageRepository _messageRepository;
    private readonly IDiscussionRepository _discussionRepository;

    public DeleteMessageHandler(
        [FromKeyedServices(ModuleKey.Discussion)] IUnitOfWork unitOfWork,
        IValidator<DeleteMessageCommand> validator,
        ILogger<DeleteMessageHandler> logger,
        IMessageRepository messageRepository, 
        IDiscussionRepository discussionRepository)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
        _messageRepository = messageRepository;
        _discussionRepository = discussionRepository;
    }

    public async Task<UnitResult<ErrorList>> HandleAsync(DeleteMessageCommand command, CancellationToken cancellationToken)
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
        
        var messageId = MessageId.Create(command.MessageId).Value;
        var message = await _messageRepository.GetByIdAsync(messageId, cancellationToken);
        if (message is null)
        {
            _logger.LogWarning($"Message with id {messageId} not found");
            return Errors.General.ValueNotFound(messageId.Value).ToErrorList();
        }
        
        var isMessageInDiscussion = discussion.IsMessageInDiscussion(message);
        if (isMessageInDiscussion.IsFailure)
            return isMessageInDiscussion.Error.ToErrorList();
        
        var isMessageBelongToUser = discussion.IsCommentBelongToUser(message, command.UserId);
        if (isMessageBelongToUser.IsFailure)
            return isMessageBelongToUser.Error.ToErrorList();
        
        var isUserInDiscussion = discussion.IsUserInDiscussion(command.UserId);
        if (isUserInDiscussion.IsFailure)
            return isUserInDiscussion.Error.ToErrorList();

        discussion.DeleteComment(message);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return Result.Success<ErrorList>();
    }
}