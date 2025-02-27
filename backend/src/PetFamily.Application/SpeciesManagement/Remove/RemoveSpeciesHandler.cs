using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;

namespace PetFamily.Application.SpeciesManagement.Remove;

public class RemoveSpeciesHandler : ICommandHandler<Guid, RemoveSpeciesCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadDbContext _context;
    private readonly ISpeciesRepository _repository;
    private readonly ILogger<RemoveSpeciesHandler> _logger;
    private readonly IValidator<RemoveSpeciesCommand> _validator;

    public RemoveSpeciesHandler(
        IUnitOfWork unitOfWork,
        ILogger<RemoveSpeciesHandler> logger,
        IValidator<RemoveSpeciesCommand> validator,
        IReadDbContext context,
        ISpeciesRepository repository)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = validator;
        _context = context;
        _repository = repository;
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

            var petWithSpecies = await _context.Pets
                .FirstOrDefaultAsync(p => p.SpeciesId == speciesId.Value, cancellationToken);
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