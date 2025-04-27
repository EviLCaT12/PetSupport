using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Discussion.Application.Database;
using PetFamily.Discussion.Domain.ValueObjects;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Discussion.Application.Commands.Create;

public class CreateHandler : ICommandHandler<Guid, CreateCommand>
{
    private readonly ILogger<CreateHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateCommand> _validator;
    private readonly IDiscussionRepository _repository;

    public CreateHandler(
        ILogger<CreateHandler> logger,
        [FromKeyedServices(ModuleKey.Discussion)] IUnitOfWork unitOfWork,
        IValidator<CreateCommand> validator,
        IDiscussionRepository repository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _repository = repository;
    }
    public async Task<Result<Guid, ErrorList>> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var transaction =  await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var discussionId = DiscussionsId.NewId();

        var discussion = Domain.Entities.Discussion.Create(
            discussionId,
            command.RelationId,
            command.Members);

        if (discussion.IsFailure)
            return discussion.Error.ToErrorList();

        await _repository.AddAsync(discussion.Value, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return discussionId.Value;
    }
}