using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Species.Contracts;
using PetFamily.Volunteers.Domain.ValueObjects.PetVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Application.Commands.UpdatePet;

public class UpdatePetHandler : ICommandHandler<Guid, UpdatePetCommand>
{
    private readonly ILogger<UpdatePetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRepository _repository;
    private readonly IReadDbContext _context;
    private readonly IValidator<UpdatePetCommand> _validator;
    private readonly ISpeciesContract _contract;

    public UpdatePetHandler(
        ILogger<UpdatePetHandler> logger,
        [FromKeyedServices(ModuleKey.Volunteer)] IUnitOfWork unitOfWork,
        IVolunteersRepository repository,
        IReadDbContext context,
        IValidator<UpdatePetCommand> validator,
        ISpeciesContract contract)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _context = context;
        _validator = validator;
        _contract = contract;
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
            
            var speciesId = command.Classification.SpeciesId;
            var breedId = command.Classification.BreedId;
            
            var isSpeciesHasBreed = await _contract.IsSpeciesHasBreed(
                speciesId,
                breedId,
                cancellationToken);

            if (isSpeciesHasBreed.IsFailure)
                return isSpeciesHasBreed.Error;
            
            var name = command.Name == null ? pet.Value.Name : Name.Create(command.Name).Value;
            
            var classification = PetClassification.Create(speciesId, breedId).Value;
            
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