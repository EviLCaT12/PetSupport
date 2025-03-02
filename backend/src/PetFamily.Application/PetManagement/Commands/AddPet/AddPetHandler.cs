using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Application.SpeciesManagement;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;
using PetFamily.Domain.SpeciesContext.ValueObjects.BreedVO;
using PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;

namespace PetFamily.Application.PetManagement.Commands.AddPet;

public class AddPetHandler : ICommandHandler<Guid, AddPetCommand>
{
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadDbContext _context;

    public AddPetHandler(
        ILogger<AddPetHandler> logger,
        IVolunteersRepository volunteersRepository,
        ISpeciesRepository speciesRepository,
        IValidator<AddPetCommand> validator,
        IUnitOfWork unitOfWork,
        IReadDbContext context)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _speciesRepository = speciesRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _context = context;
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
    
            var speciesId = SpeciesId.Create(command.Classification.SpeciesId).Value;
            var breedId = BreedId.Create(command.Classification.BreedId);

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

            List<TransferDetails> transferDetails = [];
            transferDetails.AddRange(command.TransferDetailDto
                .Select(transferDetail => TransferDetails.Create(transferDetail.Name, transferDetail.Description))
                .Select(transferDetailsCreateResult => transferDetailsCreateResult.Value));
            var transferDetailsList = new List<TransferDetails>(transferDetails);
    
            var createPetResult = Pet.Create(
                PetId.NewPetId(),
                Name.Create(command.Name).Value,
                PetClassification.Create(speciesId.Value, breedId.Value.Value).Value,
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