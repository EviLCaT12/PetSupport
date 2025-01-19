using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetHandle.ValueObjects;

public record TransferDetails
{
    public Guid IdValue { get; private set; }
    
    public string NameValue { get; private set; }
    
    public string DescriptionValue { get; private set; }

    private TransferDetails(Guid idValue, string name, string description)
    {
        IdValue = idValue;
        NameValue = name;
        DescriptionValue = description;
    }

    public static Result<TransferDetails> Create(Guid idValue, string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<TransferDetails>("Name cannot be null or empty");
            
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<TransferDetails>("Description cannot be null or empty");
        
        var transferDetails = new TransferDetails(idValue ,name, description);
        
        return Result.Success(transferDetails);
    }
    
}