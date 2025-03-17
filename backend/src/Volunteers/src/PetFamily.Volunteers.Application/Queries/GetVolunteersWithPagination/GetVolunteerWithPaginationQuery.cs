using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Queries.GetVolunteersWithPagination;

public record GetVolunteerWithPaginationQuery(int Page, int PageSize) : IQuery;
