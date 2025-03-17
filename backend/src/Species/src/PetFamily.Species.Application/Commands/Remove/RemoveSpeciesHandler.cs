using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Error;
using PetFamily.Species.Domain.ValueObjects.SpeciesVO;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Species.Application.Commands.Remove;

public class RemoveSpeciesHandler : ICommandHandler<Guid, RemoveSpeciesCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPetContract _petContract;
    private readonly ILogger<RemoveSpeciesHandler> _logger;
    private readonly IValidator<RemoveSpeciesCommand> _validator;
    private readonly ISpeciesRepository _repository;

    public RemoveSpeciesHandler(
        [FromKeyedServices(ModuleKey.Species)] IUnitOfWork unitOfWork,
        ILogger<RemoveSpeciesHandler> logger,
        IValidator<RemoveSpeciesCommand> validator,
        ISpeciesRepository speciesRepository,
        IPetContract petContract)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = validator;
        _repository = speciesRepository;
        _petContract = petContract;
    }
    public async Task<Result<Guid, ErrorList>> HandleAsync(RemoveSpeciesCommand command, CancellationToken cancellationToken)
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

            var petWithSpecies = await _petContract.IsPetHasSpecies(command.SpeciesId, cancellationToken);
            if (petWithSpecies != null)
            {
                var msg = "Species with id: " + speciesId.Value + 
                          "cannot be removed. Pet with id:" + petWithSpecies.Id + "has this species.";
                _logger.LogError(msg);
                var error = Errors.General.ValueIsInvalid(nameof(SpeciesId));
                return new ErrorList([error]);
            }


            _repository.Remove(species);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            transaction.Commit();

            return speciesId.Value;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail to remove species with id: {speciesId} during transaction ", command.SpeciesId);
            transaction.Rollback();
            
            var error = Error.Failure("species.failure",
                "Error during delete species in transaction");
            return new ErrorList([error]);
        }
    }
}