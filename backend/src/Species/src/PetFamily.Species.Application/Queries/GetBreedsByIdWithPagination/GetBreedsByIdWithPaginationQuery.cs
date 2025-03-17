using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Queries.GetBreedsByIdWithPagination;

public record GetBreedsByIdWithPaginationQuery(Guid SpeciesId, int Page, int PageSize) : IQuery;