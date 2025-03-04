using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.PetManagement.Commands.ChangePetHelpStatus;

public class ChangePetHelpStatusHandler : ICommandHandler<ChangePetHelpStatusCommand>
{
    private readonly ILogger<ChangePetHelpStatusHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRepository _repository;
    private readonly IReadDbContext _context;

    public ChangePetHelpStatusHandler(
        ILogger<ChangePetHelpStatusHandler> logger,
        IUnitOfWork unitOfWork,
        IVolunteersRepository repository,
        IReadDbContext context)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _context = context;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(ChangePetHelpStatusCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var volunteerId = VolunteerId.Create(command.VolunteerId).Value;
            var volunteer = await _repository.GetByIdAsync(volunteerId, cancellationToken);
            if (volunteer.IsFailure)
                return volunteer.Error; 
            
            var petId = PetId.Create(command.PetId).Value;
            var pet = volunteer.Value.GetPetById(petId);
            if (pet.IsFailure)
                return pet.Error;

            var parseResult = Enum.TryParse(command.HelpStatus, out HelpStatus helpStatus);
            if (parseResult == false)
            {
                var msg = $"Help status \"{command.HelpStatus}\" is not valid.";
                _logger.LogWarning(msg);
                var error = Errors.General.ValueIsInvalid(nameof(command.HelpStatus));
                return new ErrorList([error]);
            }
            
            volunteer.Value.ChangePetHelpStatus(petId, helpStatus);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            transaction.Commit();
            return Result.Success<ErrorList>();
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Fail to update pet {petId} help status {helpStatus} for volunteer {volunteerId} in transaction",
                command.PetId, command.HelpStatus, command.VolunteerId);
            
            transaction.Rollback();
            
            var error = Error.Failure("volunteer.pet.failure", "Error during update pet help status for volunteer transaction");

            return new ErrorList([error]);
        }
    }
}