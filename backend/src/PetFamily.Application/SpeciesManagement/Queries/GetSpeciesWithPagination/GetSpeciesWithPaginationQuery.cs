using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagement.Queries.GetSpeciesWithPagination;

public record GetSpeciesWithPaginationQuery(int Page, int PageSize) : IQuery;