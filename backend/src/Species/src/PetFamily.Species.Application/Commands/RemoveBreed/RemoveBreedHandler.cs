using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Error;
using PetFamily.Species.Domain.ValueObjects.BreedVO;
using PetFamily.Species.Domain.ValueObjects.SpeciesVO;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Species.Application.Commands.RemoveBreed;

public class RemoveBreedHandler : ICommandHandler<Guid, RemoveBreedCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RemoveBreedHandler> _logger;
    private readonly ISpeciesRepository _repository;
    private readonly IValidator<RemoveBreedCommand> _validator;
    private readonly IPetContract _petContract;

    public RemoveBreedHandler(
        [FromKeyedServices(ModuleKey.Species)] IUnitOfWork unitOfWork,
        ILogger<RemoveBreedHandler> logger,
        ISpeciesRepository repository,
        IValidator<RemoveBreedCommand> validator,
        IPetContract petContract)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _repository = repository;
        _validator = validator;
        _petContract = petContract;
    }
    public async Task<Result<Guid, ErrorList>> HandleAsync(RemoveBreedCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();
            
            var speciesId = SpeciesId.Create(command.SpeciesId).Value;
            var getSpeciesResult = await _repository.GetByIdAsync(speciesId, cancellationToken);
            if (getSpeciesResult.IsFailure)
            {
                _logger.LogError($"Species with id: {speciesId} not found");
                return getSpeciesResult.Error;
            }
            var species = getSpeciesResult.Value;


            var breedId = BreedId.Create(command.BreedId).Value;
            var isBreedAttachSpecies = species.GetBreedById(breedId);
            if (isBreedAttachSpecies.IsFailure)
                return isBreedAttachSpecies.Error;
            
            var petWithBreed = await _petContract.IsPetHasBreed(breedId.Value, cancellationToken);
            if (petWithBreed != null)
            {
                var msg = "Breed with id: " + command.BreedId + 
                          "cannot be removed. Pet with id:" + petWithBreed.Id + "has this species.";
                _logger.LogError(msg);
                var error = Errors.General.ValueIsInvalid(nameof(SpeciesId));
                return new ErrorList([error]);
            }
            
            species.RemoveBreed(isBreedAttachSpecies.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            transaction.Commit();

            return breedId.Value;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail to remove breed with id: {breedId} during transaction ", command.BreedId);
            transaction.Rollback();
            
            var error = Error.Failure("species.failure",
                "Error during delete breed in transaction");
            return new ErrorList([error]);
        }
    }
}