using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel.Error;
using PetFamily.Species.Application;
using PetFamily.Species.Application.Commands.AddBreeds;
using PetFamily.Species.Application.Commands.Create;
using PetFamily.Species.Contracts;
using PetFamily.Species.Contracts.Requests.Species;
using PetFamily.Species.Infrastructure.DbContexts;

namespace PetFamily.Species.Presentation.Species;

public class SpeciesContract : ISpeciesContract
{
    private readonly IReadDbContext _readContext;
    private readonly WriteDbContext _writeContext; 
    private readonly CreateHandler _createHandler;
    private readonly AddBreedsHandler _addBreedsHandler;

    public SpeciesContract(
        IReadDbContext context,
        CreateHandler createHandler,
        AddBreedsHandler addBreedsHandler,
        WriteDbContext writeContext)
    {
        _readContext = context;
        _createHandler = createHandler;
        _addBreedsHandler = addBreedsHandler;
        _writeContext = writeContext;
    }
    
    public async Task<UnitResult<ErrorList>> IsSpeciesHasBreed(
        Guid speciesId,
        Guid breedId,
        CancellationToken cancellationToken)
    {
        var species = await _readContext.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == speciesId, cancellationToken);

        if (species == null)
        {
            var error = Errors.General.ValueNotFound(speciesId);
            return new ErrorList([error]);
        }
        
        var isHasBreed = species.Breeds.FirstOrDefault(breed => breed.Id == breedId);

        if (isHasBreed == null)
        {
            var error = Errors.General.ValueNotFound(breedId);
            return new ErrorList([error]);
        }

        return UnitResult.Success<ErrorList>();
    }

    public async Task<Result<Guid, ErrorList>> AddSpecies(CreateRequest request, CancellationToken cancellationToken)
    {
        return await _createHandler.HandleAsync(
            new CreateCommand(request.Name),
            cancellationToken);
    }

    public async Task<Result<Domain.Entities.Species, ErrorList>> GetSpeciesById(Guid speciesId, CancellationToken cancellationToken)
    {
        var species = await _writeContext.Species.ToListAsync(cancellationToken);
           

        var result = species.FirstOrDefault(s => s.Id.Value == speciesId);

        if (result == null)
        {
            var error = Errors.General.ValueNotFound(speciesId);
            return new ErrorList([error]);
        }
        
        return result;
    }

    public async Task<Result<List<Guid>, ErrorList>> AddBreeds(
        Guid speciesId,
        AddBreedsRequest request,
        CancellationToken cancellationToken)
    {
        return await _addBreedsHandler.HandleAsync(
            new AddBreedsCommand(speciesId, request.Names),
            cancellationToken);
    }
    
}