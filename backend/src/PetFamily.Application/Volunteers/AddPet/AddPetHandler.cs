using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Application.Species;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;
using PetFamily.Domain.SpeciesContext.ValueObjects.BreedVO;
using PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;

namespace PetFamily.Application.Volunteers.AddPet;

public class AddPetHandler
{
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public AddPetHandler(
        ILogger<AddPetHandler> logger,
        IVolunteersRepository volunteersRepository,
        ISpeciesRepository speciesRepository,
        IValidator<AddPetCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _speciesRepository = speciesRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
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
    
            var speciesId = SpeciesId.Create(command.Classification.SpeciesId);
            var breedId = BreedId.Create(command.Classification.BreedId);
            var checkSpeciesAndBreedResult = await _speciesRepository.GetBreedByIdAsync(
                speciesId,
                breedId,
                cancellationToken);
            if (checkSpeciesAndBreedResult.IsFailure)
                return checkSpeciesAndBreedResult.Error;
    
            List<TransferDetails> transferDetails = [];
            transferDetails.AddRange(command.TransferDetailDto
                .Select(transferDetail => TransferDetails.Create(transferDetail.Name, transferDetail.Description))
                .Select(transferDetailsCreateResult => transferDetailsCreateResult.Value));
            var transferDetailsList = new ValueObjectList<TransferDetails>(transferDetails);
    
            var createPetResult = Pet.Create(
                PetId.NewPetId(),
                Name.Create(command.Name).Value,
                PetClassification.Create(speciesId.Value, breedId.Value).Value,
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
            
            return volunteer.Id.Value;
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