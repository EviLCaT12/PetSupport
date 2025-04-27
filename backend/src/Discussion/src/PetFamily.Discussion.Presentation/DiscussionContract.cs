using Contracts;
using CSharpFunctionalExtensions;
using PetFamily.Discussion.Domain.ValueObjects;
using PetFamily.SharedKernel.Error;

namespace PetFamily.Discussion.Presentation;

public class DiscussionContract : IDiscussionContract
{
    public Result<Guid, ErrorList> CreateDiscussionForVolunteerRequest(Guid requestId, IEnumerable<Guid> membersId)
    {
        //Fixme: Поменять на полноценный хендлер в следующем задании
        var discussion = Domain.Entities.Discussion.Create(
            DiscussionsId.NewId(),
            requestId,
            membersId);

        if (discussion.IsFailure)
            return discussion.Error.ToErrorList();

        return discussion.Value.Id.Value;
    }
}