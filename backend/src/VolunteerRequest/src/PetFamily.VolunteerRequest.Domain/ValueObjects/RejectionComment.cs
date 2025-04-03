using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;

namespace PetFamily.VolunteerRequest.Domain.ValueObjects;

public class RejectionComment : ValueObject
{
    private RejectionComment() { }

    private RejectionComment(
        Guid adminId,
        Guid userId,
        Description description)
    {
        AdminId = adminId;
        UserId = userId;
        Description = description;
    }
    
    public Guid AdminId { get; }
    
    public Guid UserId { get; }
    
    public DateTime RejectionDate { get; } = DateTime.UtcNow;
    
    public Description Description { get; }

    public static Result<RejectionComment, ErrorList> Create(
        Guid adminId,
        Guid userId,
        Description description)
    {
        if (adminId == Guid.Empty)
            return Errors.General.ValueIsRequired(nameof(adminId)).ToErrorList();
        
        if (userId == Guid.Empty)
            return Errors.General.ValueIsRequired(nameof(userId)).ToErrorList();
        
        if (adminId == userId)
            return Errors.General.ValueIsInvalid(nameof(userId)).ToErrorList();
        
        return new RejectionComment(adminId, userId, description);
    }
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return AdminId;
        
        yield return UserId;
        
        yield return RejectionDate;
    }
}