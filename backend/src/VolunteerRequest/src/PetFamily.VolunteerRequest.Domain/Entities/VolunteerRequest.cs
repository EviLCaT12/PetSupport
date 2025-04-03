using CSharpFunctionalExtensions;
using PetFamily.VolunteerRequest.Domain.Enums;
using PetFamily.VolunteerRequest.Domain.ValueObjects;

namespace PetFamily.VolunteerRequest.Domain.Entities;

public class VolunteerRequest : Entity <VolunteerRequestId>
{
    private VolunteerRequest() { }

    public VolunteerRequest(
        VolunteerRequestId id,
        Guid adminId,
        Guid userId,
        Guid discussionId,
        VolunteerInfo volunteerInfo)
    {
        Id = id;
        AdminId = adminId;
        UserId = userId;
        DiscussionId = discussionId;
        VolunteerInfo = volunteerInfo;
    }
    
    public VolunteerRequestId Id { get; private set; }
    
    public Guid AdminId { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public Guid DiscussionId { get; private set; }
    
    public VolunteerInfo VolunteerInfo { get; private set; }

    public Status Status { get; private set; } = Status.Submitted;
    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public RejectionComment? RejectionComment { get; private set; } = null;



}
