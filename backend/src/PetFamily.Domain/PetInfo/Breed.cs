using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetInfo;

public class Breed
{
    public Guid Id { get; private set; }
    
    public string Name { get; private set; }

    private Breed(string name)
    {
        Name = name;
    }

    public static Result<Breed> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Breed>("Breed name cannot be null or empty");

        var breed = new Breed(name);
        
        return Result.Success(breed);
    }
}