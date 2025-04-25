using PetFamily.Core.Abstractions;

namespace PetFamily.Discussion.Application.Queries.GetDiscussionWIthAllMsgByRelationId;

public record GetDiscussionWithAllMsgByRelationIdQuery(Guid RelationId) : IQuery;