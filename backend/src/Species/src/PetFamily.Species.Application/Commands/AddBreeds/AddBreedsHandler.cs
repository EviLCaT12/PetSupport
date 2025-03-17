using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Species.Domain.Entities;
using PetFamily.Species.Domain.ValueObjects.BreedVO;
using PetFamily.Species.Domain.ValueObjects.SpeciesVO;

namespace PetFamily.Species.Application.Commands.AddBreeds;

public class AddBreedsHandler : ICommandHandler<List<Guid>, AddBreedsCommand>
{
    private readonly ILogger<AddBreedsHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISpeciesRepository _repository;
    private readonly IValidator<AddBreedsCommand> _validator;

    public AddBreedsHandler(
        ILogger<AddBreedsHandler> logger,
        [FromKeyedServices(ModuleKey.Species)] IUnitOfWork unitOfWork,
        ISpeciesRepository repository,
        IValidator<AddBreedsCommand> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _validator = validator;
    }
    public async Task<Result<List<Guid>, ErrorList>> HandleAsync(AddBreedsCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var speciesId = SpeciesId.Create(command.SpeciesId);
            var getSpeciesResult = await _repository.GetByIdAsync(speciesId.Value, cancellationToken);
            if (getSpeciesResult.IsFailure)
            {
                _logger.LogError($"Species with id: {speciesId} not found");
                return getSpeciesResult.Error;
            }
            
            var species = getSpeciesResult.Value;
            
            List<Breed> breeds = [];
            foreach (var name in command.Names)
            {
                var breedId = BreedId.NewBreedId();
                var breedName = Name.Create(name).Value;
                var breed = Breed.Create(breedId, breedName).Value;
                breeds.Add(breed);
            }
            
            species.AddBreeds(breeds);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            transaction.Commit();
            
            return breeds.Select(bred => bred.Id.Value).ToList();
        }
        catch (Exception e)
        {
            var msg = "An error occured while adding breeds";
            _logger.LogError(e, msg);
            transaction.Rollback();
            var error = Error.Failure("server.error", msg);
            return new ErrorList([error]);
        }
    }
}