using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.VolunteerRequest.Application.Abstractions;
using PetFamily.VolunteerRequest.Domain.ValueObjects;

namespace PetFamily.VolunteerRequest.Application.Commands.SendRequestToRevision;

public class SendRequestToRevisionHandler : ICommandHandler<SendRequestToRevisionCommand>
{
    private readonly IValidator<SendRequestToRevisionCommand> _validator;
    private readonly ILogger<SendRequestToRevisionHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteerRequestRepository _repository;

    public SendRequestToRevisionHandler(
        IValidator<SendRequestToRevisionCommand> validator,
        ILogger<SendRequestToRevisionHandler> logger,
        [FromKeyedServices(ModuleKey.VolunteerRequest)] IUnitOfWork unitOfWork,
        IVolunteerRequestRepository repository)
    {
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _repository = repository;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(SendRequestToRevisionCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var requestId = VolunteerRequestId.Create(command.RequestId).Value;
        var request = await _repository.GetVolunteerRequestByIdAsync(requestId, cancellationToken);
        if (request is null)
        {
            _logger.LogError($"Request with id: {requestId} not found");
            return Errors.General.ValueNotFound(command.RequestId).ToErrorList();
        }
            
        var comment = new RejectionComment(Description.Create(command.Discription).Value);
        var result = request.SendForRevision(comment);
        if (result.IsFailure)
            return result.Error;
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return UnitResult.Success<ErrorList>();
        
    }
}