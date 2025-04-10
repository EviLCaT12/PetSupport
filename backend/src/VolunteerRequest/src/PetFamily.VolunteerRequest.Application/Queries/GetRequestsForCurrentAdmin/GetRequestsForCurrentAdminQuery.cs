using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequest.Application.Queries.GetRequestsForCurrentAdmin;

public record GetRequestsForCurrentAdminQuery(
    Guid AdminId,
    string Status, 
    int Page,
    int PageSize) : IQuery;