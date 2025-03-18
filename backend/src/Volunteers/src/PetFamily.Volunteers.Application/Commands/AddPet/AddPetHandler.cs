using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Species.Contracts;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.ValueObjects.PetVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Application.Commands.AddPet;

public class AddPetHandler : ICommandHandler<Guid, AddPetCommand>
{
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ISpeciesContract _contract;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public AddPetHandler(
        ILogger<AddPetHandler> logger,
        IVolunteersRepository volunteersRepository,
        IValidator<AddPetCommand> validator,
        [FromKeyedServices(ModuleKey.Volunteer)] IUnitOfWork unitOfWork,
        ISpeciesContract contract)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _contract = contract;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(AddPetCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        { 
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();
            
            var getVolunteerResult = await _volunteersRepository
                .GetByIdAsync(VolunteerId.Create(command.VolunteerId).Value, cancellationToken);
            if (getVolunteerResult.IsFailure)
                return getVolunteerResult.Error;
            
            var speciesId = command.Classification.SpeciesId;
            var breedId = command.Classification.BreedId;
            
            var isSpeciesHasBreed = await _contract.IsSpeciesHasBreed(
                speciesId,
                breedId,
                cancellationToken);

            if (isSpeciesHasBreed.IsFailure)
                return isSpeciesHasBreed.Error;
            
            List<TransferDetails> transferDetails = [];
            transferDetails.AddRange(command.TransferDetailDto
                .Select(transferDetail => TransferDetails.Create(transferDetail.Name, transferDetail.Description))
                .Select(transferDetailsCreateResult => transferDetailsCreateResult.Value));
            var transferDetailsList = new List<TransferDetails>(transferDetails);
    
            var createPetResult = Pet.Create(
                PetId.NewPetId(),
                Name.Create(command.Name).Value,
                PetClassification.Create(speciesId, breedId).Value,
                Description.Create(command.Description).Value,
                Color.Create(command.Color).Value,
                HealthInfo.Create(command.HealthInfo).Value,
                Address.Create(command.Address.City, command.Address.Street, command.Address.HouseNumber).Value,
                Dimensions.Create(command.Dimensions.Height, command.Dimensions.Weight).Value,
                Phone.Create(command.OwnerPhone).Value,
                command.IsCastrate,
                command.DateOfBirth,
                command.IsVaccinated,
                command.HelpStatus,
                transferDetailsList,
                new List<PetPhoto>());
            
            var volunteer = getVolunteerResult.Value;
            volunteer.AddPet(createPetResult.Value);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            transaction.Commit();
            
            return createPetResult.Value.Id.Value;
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Fail to add pet to volunteer {volunteerId} in transaction", command.VolunteerId);
            
            transaction.Rollback();
            
            var error = Error.Failure("volunteer.pet.failure", "Error during add pet to volunteer transaction");

            return new ErrorList([error]);
        }
        
    }
}