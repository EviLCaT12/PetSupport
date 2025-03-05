using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.PetManagement.Commands.UpdatePet;

public class UpdatePetHandler : ICommandHandler<Guid, UpdatePetCommand>
{
    private readonly ILogger<UpdatePetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRepository _repository;
    private readonly IReadDbContext _context;
    private readonly IValidator<UpdatePetCommand> _validator;

    public UpdatePetHandler(
        ILogger<UpdatePetHandler> logger,
        IUnitOfWork unitOfWork,
        IVolunteersRepository repository,
        IReadDbContext context,
        IValidator<UpdatePetCommand> validator
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _context = context;
        _validator = validator;
    }
    public async Task<Result<Guid, ErrorList>> HandleAsync(UpdatePetCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();
            
            var volunteerId = VolunteerId.Create(command.VolunteerId).Value;
            var volunteer = await _repository.GetByIdAsync(volunteerId, cancellationToken);
            if (volunteer.IsFailure)
                return volunteer.Error;
            
            var petId = PetId.Create(command.PetId).Value;
            var pet = volunteer.Value.GetPetById(petId);
            if (pet.IsFailure)
                return pet.Error;
            

            var getSpeciesResult = await _context.Species
                .Include(s => s.Breeds)
                .FirstOrDefaultAsync(s => s.Id == command.Classification.SpeciesId, cancellationToken);
            if (getSpeciesResult is null)
            {
                var msg = $"Species {command.Classification.SpeciesId} not found";
                _logger.LogError(msg);
                var error = Errors.General.ValueNotFound(command.Classification.SpeciesId);
                return new ErrorList([error]);
            }
            
            var isSpeciesHasBreed = getSpeciesResult.Breeds
                .FirstOrDefault(b => b.Id == command.Classification.BreedId);
            if (isSpeciesHasBreed is null)
            {
                var msg = $"Breed {command.Classification.BreedId} not found";
                _logger.LogError(msg);
                var error = Errors.General.ValueNotFound(command.Classification.BreedId);
                return new ErrorList([error]);
            }
            
            var name = command.Name == null ? pet.Value.Name : Name.Create(command.Name).Value;
            
            var classification = PetClassification.Create(command.Classification.SpeciesId, command.Classification.BreedId).Value;
            
            var description = command.Description == null ? pet.Value.Description : Description.Create(command.Description).Value;
            
            var color = command.Color == null ? pet.Value.Color : Color.Create(command.Color).Value;
            
            var health = command.HealthInfo == null ? pet.Value.HealthInfo : HealthInfo.Create(command.HealthInfo).Value;

            var address = command.Address == null ? pet.Value.Address : Address.Create(command.Address.City, command.Address.Street, command.Address.Street).Value;

            var dimensions = command.Dimensions == null
                ? pet.Value.Dimensions
                : Dimensions.Create(command.Dimensions.Height, command.Dimensions.Weight).Value;
            
            var ownerPhoneNumber = command.OwnerPhone == null ? pet.Value.OwnerPhoneNumber : Phone.Create(command.OwnerPhone).Value;
            
            var isCastrate = command.IsCastrate ?? pet.Value.IsCastrate;
            
            var dateOfBirth = command.DateOfBirth ?? pet.Value.DateOfBirth;
            
            var isVaccinated = command.IsVaccinated ?? pet.Value.IsVaccinated;

            var helpStatus = command.HelpStatus ?? (int)pet.Value.HelpStatus;

            var transferDetailsList = command.TransferDetailsDto == null
                ? pet.Value.TransferDetailsList
                : command.TransferDetailsDto
                    .Select(dto => TransferDetails.Create(dto.Name, dto.Description).Value);
            
            volunteer.Value.UpdatePet(
                pet.Value,
                name,
                classification,
                description,
                color,
                health,
                address,
                dimensions,
                ownerPhoneNumber,
                isCastrate,
                dateOfBirth,
                isVaccinated,
                helpStatus,
                transferDetailsList
                );
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            transaction.Commit();
            return petId.Value;
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Fail to update pet {petId} for volunteer {volunteerId} in transaction", command.PetId ,command.VolunteerId);
            
            transaction.Rollback();
            
            var error = Error.Failure("volunteer.pet.failure", "Error during update pet for volunteer transaction");

            return new ErrorList([error]);
        }
    }
}