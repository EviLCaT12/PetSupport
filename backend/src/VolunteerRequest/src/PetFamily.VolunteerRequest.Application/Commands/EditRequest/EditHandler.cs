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

namespace PetFamily.VolunteerRequest.Application.Commands.EditRequest;

public class EditHandler : ICommandHandler<EditCommand>
{
    private readonly IValidator<EditCommand> _validator;
    private readonly ILogger<EditHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteerRequestRepository _repository;

    public EditHandler(
        IValidator<EditCommand> validator,
        ILogger<EditHandler> logger,
        [FromKeyedServices(ModuleKey.VolunteerRequest)] IUnitOfWork unitOfWork,
        IVolunteerRequestRepository repository)
    {
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _repository = repository;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(EditCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();
            
            var request = await _repository.GetVolunteerRequestByIdAsync(
                VolunteerRequestId.Create(command.RequestId).Value,
                cancellationToken);
            if (request is null)
            {
                _logger.LogError($"Request with id: {command.RequestId} not found");
                return Errors.General.ValueNotFound(command.RequestId).ToErrorList();
            }

            var newVolunteerInfo = new VolunteerInfo(
                Fio.Create(command.Fio.FirstName, command.Fio.LastName, command.Fio.SurName).Value,
                Description.Create(command.Description).Value,
                Email.Create(command.Email).Value,
                YearsOfExperience.Create(command.Experience).Value);

            var result = request.Edit(newVolunteerInfo);
            if (result.IsFailure)
                return result.Error;
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            transaction.Commit();

            return UnitResult.Success<ErrorList>();

        }
        catch (Exception e)
        {
            transaction.Rollback();
            _logger.LogError(e, "Unexpected error occured during edit volunteer request");
            return Errors.General.ErrorDuringTransaction();
        }
    }
}