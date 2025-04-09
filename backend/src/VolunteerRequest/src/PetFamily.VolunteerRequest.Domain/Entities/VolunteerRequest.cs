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
    
    public DateTime? RejectionDate{ get; private set; }

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
    public UnitResult<ErrorList> SendForRevision(RejectionComment comment)
    {
        //Проверка на нул не требуется, так как заявка и коммент никогда нул не будут в силу методов их создания
        if (Status == Status.RevisionRequired)
            return Errors.VolunteerRequest.RequestAlreadySendForRevision();
        
        Status = Status.RevisionRequired;
        
        RejectionComment = comment;

        return UnitResult.Success<ErrorList>();
    }

    //Отменить заявку
    public UnitResult<ErrorList> RejectRequest(RejectionComment comment)
    {
        if (Status == Status.Rejected)
            return Errors.VolunteerRequest.RequestAlreadyRejected();
        
        RejectionDate = DateTime.UtcNow;
        
        RejectionComment = comment;
        
        Status = Status.Rejected;
        
        return UnitResult.Success<ErrorList>();
    }

    //Утвердить заявку
    public UnitResult<ErrorList> ApproveRequest()
    {
        if (Status == Status.Approved)
            return Errors.VolunteerRequest.RequestAlreadyApproved();
        
        Status = Status.Approved;
        
        return UnitResult.Success<ErrorList>();
    }

    //Пока самый простой метод сугубо для тестов
    public VolunteerRequest AddRejectionComment(RejectionComment comment)
    {
        RejectionComment = comment;
        return this;
    }
    
}
