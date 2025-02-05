using CSharpFunctionalExtensions;
    
namespace PetFamily.Domain.Shared.SharedVO;

public record TransferDetailsList
{
    private readonly List<TransferDetails> _transferDetails;
    
    public IReadOnlyList<TransferDetails> TransferDetails => _transferDetails;

    private TransferDetailsList(IEnumerable<TransferDetails> transferDetails)
    {
        _transferDetails = transferDetails.ToList();
    }

    //ef core
    private TransferDetailsList() { }
    
    public static Result<TransferDetailsList, Error.Error> Create(List<TransferDetails> transferDetails) =>
        new TransferDetailsList(transferDetails);
}