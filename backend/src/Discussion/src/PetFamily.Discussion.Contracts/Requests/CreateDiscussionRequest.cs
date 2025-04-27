namespace Contracts.Requests;

public record CreateDiscussionRequest(Guid RequestId, IEnumerable<Guid> MembersId);