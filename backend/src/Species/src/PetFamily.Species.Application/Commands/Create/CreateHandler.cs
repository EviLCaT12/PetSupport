using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Species.Domain.ValueObjects.SpeciesVO;

namespace PetFamily.Species.Application.Commands.Create;

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
        [FromKeyedServices(ModuleKey.Species)] IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
    
        
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var speciesId = SpeciesId.NewSpeciesId();
            var speciesName = Name.Create(command.Name).Value;
            
            var species = Domain.Entities.Species.Create(speciesId, speciesName).Value;
            
            await _repository.AddAsync(species, cancellationToken);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            transaction.Commit();
            
            return speciesId.Value;
 
        }
}