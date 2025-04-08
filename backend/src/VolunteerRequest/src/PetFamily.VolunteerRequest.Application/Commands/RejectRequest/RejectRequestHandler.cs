using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Contracts.Requests;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.VolunteerRequest.Application.Abstractions;
using PetFamily.VolunteerRequest.Domain.ValueObjects;

namespace PetFamily.VolunteerRequest.Application.Commands.RejectRequest;

public class RejectRequestHandler : ICommandHandler<RejectRequestCommand>
{
    private readonly ILogger<RejectRequestHandler> _logger;
    private readonly IValidator<RejectRequestCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteerRequestRepository _repository;
    private readonly IAccountContract _accountContract;

    public RejectRequestHandler(
        ILogger<RejectRequestHandler> logger,
        IValidator<RejectRequestCommand> validator,
        [FromKeyedServices(ModuleKey.VolunteerRequest)] IUnitOfWork unitOfWork,
        IVolunteerRequestRepository repository,
        IAccountContract accountContract)
    {
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _accountContract = accountContract;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(RejectRequestCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var id = VolunteerRequestId.Create(command.RequestId).Value;
            var request = await _repository.GetVolunteerRequestByIdAsync(id, cancellationToken);
            if (request is null)
            {
                _logger.LogError($"Request with id: {id} not found");
                return Errors.General.ValueNotFound(command.RequestId).ToErrorList();
            }

            var description = Description.Create(command.Description).Value;
            var rejectComment = new RejectionComment(description);
            
            var result = request.RejectRequest(rejectComment);
            if (result.IsFailure)
                return result.Error;

            //Баним отправку пользователю на 7 дней
            var banUserResult = await _accountContract.BanUserToSendVolunteerRequest(
                new BanUserToSendVolunteerRequestRequest(request.UserId),
                cancellationToken);
            if (banUserResult.IsFailure)
                return banUserResult.Error;
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            transaction.Commit();

            return UnitResult.Success<ErrorList>();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            _logger.LogError(e, "Unexpected error occured during transaction");
            return Errors.General.ErrorDuringTransaction();
        }
    }
}