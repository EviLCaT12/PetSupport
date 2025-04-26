using Contracts;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Error;
using PetFamily.VolunteerRequest.Application.Abstractions;
using PetFamily.VolunteerRequest.Domain.ValueObjects;

namespace PetFamily.VolunteerRequest.Application.Commands.TakeRequestOnReview;

public class TakeRequestOnReviewHandler : ICommandHandler<TakeRequestOnReviewCommand>
{
    private readonly ILogger<TakeRequestOnReviewHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<TakeRequestOnReviewCommand> _validator;
    private readonly IDiscussionContract _contract;
    private readonly IVolunteerRequestRepository _repository;

    public TakeRequestOnReviewHandler(
        ILogger<TakeRequestOnReviewHandler> logger,
        [FromKeyedServices(ModuleKey.VolunteerRequest)] IUnitOfWork unitOfWork,
        IValidator<TakeRequestOnReviewCommand> validator, 
        IDiscussionContract contract,
        IVolunteerRequestRepository repository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _contract = contract;
        _repository = repository;
    }
    
    public async Task<UnitResult<ErrorList>> HandleAsync(TakeRequestOnReviewCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var requestId = VolunteerRequestId.Create(command.RequestId).Value;
        var request = await _repository
            .GetVolunteerRequestByIdAsync(requestId, cancellationToken);
        if (request == null)
            return Errors.General.ValueNotFound(command.RequestId).ToErrorList();


        var members = new List<Guid>() { request.UserId, command.AdminId };
        var discussion = _contract.CreateDiscussionForVolunteerRequest(
            request.Id.Value,
            members);
        if (discussion.IsFailure)
            return discussion.Error;

        var result = request.TakeRequestOnReview(command.AdminId, discussion.Value);
        if (result.IsFailure)
            return result.Error;
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return UnitResult.Success<ErrorList>();
        
    }
}