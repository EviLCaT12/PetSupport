using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerRequest.Application.Queries.GetAllSubmittedRequestsWithPagination;

public record GetAllSubmittedRequestQuery(int Page, int PageSize) : IQuery;