using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Queries.GetVolunteersWithPagination;

public record GetVolunteerWithPaginationQuery(int Page, int PageSize) : IQuery;
