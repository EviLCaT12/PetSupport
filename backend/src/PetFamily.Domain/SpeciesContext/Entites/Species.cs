using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;
using PetFamily.Domain.SpeciesContext.ValueObjects.SpeciesVO;

namespace PetFamily.Domain.SpeciesContext.Entites;

public class Species : Entity<SpeciesId>
{
    public SpeciesId Id { get; private set; }
    
    public Name Name { get; private set; }
    
    private readonly List<Breed> _breeds;

    public IReadOnlyList<Breed> Breeds => _breeds;
    

    //ef core
    private Species() {}
    private Species(SpeciesId id, Name name, List<Breed> breeds)
    {
        Id = id;
        Name = name;
        _breeds = breeds;
    }

    public static Result<Species, Error> Create(SpeciesId id, Name name, List<Breed> breeds)
    {

        var species = new Species(id, name, breeds);
        
        return species; 
    }
}