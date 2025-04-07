using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.VolunteerRequest.Domain.ValueObjects;

public class RejectionComment : ValueObject
{
    private RejectionComment() { }

    public RejectionComment(Description description)
    {
        Description = description;
    }
    public Description Description { get; }
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Description;
    }
}