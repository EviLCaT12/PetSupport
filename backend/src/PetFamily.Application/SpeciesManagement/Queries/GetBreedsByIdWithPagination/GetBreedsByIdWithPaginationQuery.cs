using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagement.Queries.GetBreedsByIdWithPagination;

public record GetBreedsByIdWithPaginationQuery(Guid SpeciesId, int Page, int PageSize) : IQuery;