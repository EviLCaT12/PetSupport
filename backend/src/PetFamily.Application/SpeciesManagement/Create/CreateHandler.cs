using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;

namespace PetFamily.Application.SpeciesManagement.Create;

public class CreateHandler : ICommandHandler<Guid, CreateCommand>
{
    private readonly IValidator<CreateCommand> _validator;
    private readonly ILogger<CreateHandler> _logger;
    private readonly ISpeciesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHandler(
        IValidator<CreateCommand> validator,
        ILogger<CreateHandler> logger,
        ISpeciesRepository repository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
    
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                if (validationResult.IsValid == false)
                    return validationResult.ToErrorList();
    
                var speciesId = SpeciesId.NewSpeciesId();
                var speciesName = Name.Create(command.Name).Value;
                
                var species = Species.Create(speciesId, speciesName).Value;
                
                await _repository.AddAsync(species, cancellationToken);
                
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                
                transaction.Commit();
                
                return speciesId.Value;
            }
            catch (Exception e)
            {
                var msg = $"An Error occured while creating species in transaction: {e.Message}";
                _logger.LogError(msg);
                var error = Error.Failure("server.error", msg);
                return new ErrorList([error]);
            } 
        }
}