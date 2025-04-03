using PetFamily.SharedKernel.SharedVO;
using PetFamily.VolunteerRequest.Domain.Entities;
using PetFamily.VolunteerRequest.Domain.ValueObjects;

namespace UnitTests;

public class Utilities
{
    public static List<VolunteerRequest> CreateVolunteerRequestWithoutRejectionComment(int count, RejectionComment? comment = null)
    {
        List<VolunteerRequest> requests = [];
        for (var i = 0; i < count; i++)
        {
            var requestId = VolunteerRequestId.NewVolunteerRequestId();
        
            var adminId = Guid.NewGuid();

            var userId = Guid.NewGuid();
        
            var discussionId = Guid.NewGuid();

            var info = new VolunteerInfo(
                Fio.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()).Value,
                Description.Create(Guid.NewGuid().ToString()).Value,
                Email.Create("email" + Guid.NewGuid() + "@email.com").Value,
                Photo.Create(FilePath.Create("file.pdf", null).Value).Value,
                YearsOfExperience.Create(5).Value);

            var request = new VolunteerRequest(
                requestId,
                adminId,
                userId,
                discussionId,
                info);
            
            requests.Add(request);
        }
        
        return requests;
    }

    public static List<VolunteerRequest> CreateVolunteerRequestWithRejectionComment(int count)
    {
        List<VolunteerRequest> requests = [];
        
        for (var i = 0; i < count; i++)
        {
            var comment = RejectionComment.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Description.Create(Guid.NewGuid().ToString()).Value).Value;
            
            var request = CreateVolunteerRequestWithoutRejectionComment(
                1,
                comment).First();
            
            request.AddRejectionComment(comment);
            
            requests.Add(request);
        }
        
        return requests;
    }

    public static RejectionComment CreateValidRejectionComment(
        Guid? adminId,
        Guid? userId,
        Description? description)
    {
        var admin = adminId ?? Guid.NewGuid();
        
        var user = userId ?? Guid.NewGuid();
        
        var validDescription = description ?? Description.Create(Guid.NewGuid().ToString()).Value;
        
        var comment = RejectionComment.Create(
            admin,
            user,
            validDescription).Value;

        return comment;
    }
}