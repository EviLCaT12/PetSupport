using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;
using PetFamily.VolunteerRequest.Domain.Enums;
using PetFamily.VolunteerRequest.Domain.ValueObjects;

namespace PetFamily.VolunteerRequest.Domain.Entities;

public class VolunteerRequest : Entity <VolunteerRequestId>
{
    private VolunteerRequest() { }

    public VolunteerRequest(
        VolunteerRequestId id,
        Guid userId,
        VolunteerInfo volunteerInfo)
    {
        Id = id;
        UserId = userId;
        VolunteerInfo = volunteerInfo;
    }
    
    public VolunteerRequestId Id { get; private set; }
    
    public Guid? AdminId { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public Guid? DiscussionId { get; private set; }
    
    public VolunteerInfo VolunteerInfo { get; private set; }

    public Status Status { get; private set; } = Status.Submitted;
    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    
    public DateTime? RejectionDate { get; }

    public RejectionComment? RejectionComment { get; private set; }


    //Взять заявку в рассмотрение
    public UnitResult<ErrorList> TakeRequestOnReview(Guid adminId, Guid discussionId)
    {
        if (Status == Status.OnReview)
            return Errors.VolunteerRequest.RequestAlreadyOnReview();
            
        AdminId = adminId;
        DiscussionId = discussionId;
        Status = Status.OnReview;

        return UnitResult.Success<ErrorList>();
    }

    //Отправить на доработку
    public VolunteerRequest SendForRevision(RejectionComment comment)
    {
        //Проверка на нул не требуется, так как заявка и коммент никогда нул не будут в силу методов их создания
        Status = Status.RevisionRequired;
        
        RejectionComment = comment;

        return this;
    }

    //Отменить заявку
    public VolunteerRequest RejectRequest(RejectionComment? comment)
    {
        if (comment is not null)
            RejectionComment = comment;
        
        Status = Status.Rejected;
        
        return this;
    }

    //Утвердить заявку
    public VolunteerRequest ApproveRequest()
    {
        Status = Status.Approved;
        
        return this;
    }

    //Пока самый простой метод сугубо для тестов
    public VolunteerRequest AddRejectionComment(RejectionComment comment)
    {
        RejectionComment = comment;
        return this;
    }
    
}
