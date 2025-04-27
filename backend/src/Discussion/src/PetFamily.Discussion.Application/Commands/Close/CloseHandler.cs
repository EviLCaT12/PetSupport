using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussion.Application.Database;
using PetFamily.Discussion.Domain.ValueObjects;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Discussion.Application.Commands.Close;

public class CloseHandler : ICommandHandler<CloseCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CloseCommand> _validator;
    private readonly ILogger<CloseHandler> _logger;
    private readonly IDiscussionRepository _repository;

    public CloseHandler(
        [FromKeyedServices(ModuleKey.Discussion)] IUnitOfWork unitOfWork,
        IValidator<CloseCommand> validator,
        ILogger<CloseHandler> logger,
        IDiscussionRepository repository)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
        _repository = repository;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(CloseCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var discussionId = DiscussionsId.Create(command.DiscussionId).Value;
        var discussion = await _repository.GetByIdAsync(discussionId, cancellationToken);
        if (discussion is null)
        {
            _logger.LogWarning($"Discussion with id {discussionId} not found");
            return Errors.General.ValueNotFound(discussionId.Value).ToErrorList();
        }
        
        var isUserInDiscussion = discussion.IsUserInDiscussion(command.UserId);
        if (isUserInDiscussion.IsFailure)
            return isUserInDiscussion.Error.ToErrorList();
        
        discussion.Close();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();
        
        return Result.Success<ErrorList>();
    }
}