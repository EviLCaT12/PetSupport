using Contracts;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Error;
using PetFamily.VolunteerRequest.Application.Abstractions;
using PetFamily.VolunteerRequest.Domain.ValueObjects;

namespace PetFamily.VolunteerRequest.Application.Commands.ApproveRequest;

public class ApproveRequestHandler : ICommandHandler<ApproveRequestCommand>
{
    private readonly IValidator<ApproveRequestCommand> _validator;
    private readonly ILogger<ApproveRequestHandler> _logger;
    private readonly IAccountContract _accountContract;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteerRequestRepository _repository;

    public ApproveRequestHandler(
        IValidator<ApproveRequestCommand> validator,
        ILogger<ApproveRequestHandler> logger,
        IAccountContract accountContract,
        [FromKeyedServices(ModuleKey.VolunteerRequest)] IUnitOfWork unitOfWork,
        IVolunteerRequestRepository repository)
    {
        _validator = validator;
        _logger = logger;
        _accountContract = accountContract;
        _unitOfWork = unitOfWork;
        _repository = repository;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(ApproveRequestCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validatorResult.IsValid == false)
                return validatorResult.ToErrorList();
            
            var requestId = VolunteerRequestId.Create(command.RequestId).Value;
            var request = await _repository.GetVolunteerRequestByIdAsync(requestId, cancellationToken);
            if (request is null)
            {
                _logger.LogError($"Request with id: {requestId} not found");
                return Errors.General.ValueNotFound(command.RequestId).ToErrorList();
            }


            var result = request.ApproveRequest();
            if (result.IsFailure)
                return result.Error;
            
            //Создаем аккаунт волонтера
            var createVolunteerAccountResult = await _accountContract.CreateVolunteerAccount(
                new ApproveRequestRequest(
                    request.UserId,
                    command.PhoneNumber,
                    request.VolunteerInfo.Experience.Value,
                    request.VolunteerInfo.Description.Value), 
                cancellationToken);
            
            if (createVolunteerAccountResult.IsFailure)
                return createVolunteerAccountResult.Error;
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            transaction.Commit();
            
            return UnitResult.Success<ErrorList>();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            _logger.LogError(e, "An error occured while approving a request");
            return Errors.General.ErrorDuringTransaction();
        }
    }
}