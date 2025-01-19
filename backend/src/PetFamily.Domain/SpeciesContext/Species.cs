using CSharpFunctionalExtensions;

namespace PetFamily.Domain.SpeciesContext;

public class Species
{
    public Guid Id { get; private set; }
    
    public string Name { get; private set; }

    private readonly List<Breed> _breeds;

    public IReadOnlyList<Breed> Breeds => _breeds;

    private Species(string name, List<Breed> breeds)
    {
        Name = name;
        _breeds = breeds;
    }

    public static Result<Species> Create(string name, List<Breed> breeds)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Species>("Name cannot be null or empty.");

        var species = new Species(name, breeds);
        
        return Result.Success(species); 
    }
}