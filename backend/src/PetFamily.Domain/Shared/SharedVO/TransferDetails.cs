using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.Shared.SharedVO;

public record TransferDetails
{
    public TransferDetails() {}
    private TransferDetails(string name, string description)
    {
        Name = name;
        Description = description;
    }
    
    public string Name { get; private set; }
    
    public string Description { get; private set; }

    public static Result<TransferDetails, Error.Error> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsRequired(nameof(Name));
            
        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsRequired(nameof(Description));
        
        var validTransferDetails = new TransferDetails(name, description);
        
        return validTransferDetails;
    }
    
}