using CSharpFunctionalExtensions;

namespace PetFamily.Domain.SharedVO;

public record TransferDetails
{
    public string Name { get; private set; }
    
    public string Description { get; private set; }

    private TransferDetails(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static Result<TransferDetails> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<TransferDetails>("Name cannot be null or empty");
            
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<TransferDetails>("Description cannot be null or empty");
        
        var transferDetails = new TransferDetails(name, description);
        
        return Result.Success(transferDetails);
    }
    
}