using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequest.Application.Queries.GetRequestsForCurrentUser;

public record GetRequestsForCurrentUserQuery(
    Guid UserId,
    string Status,
    int Page,
    int PageSize) : IQuery;