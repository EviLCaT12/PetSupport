using PetFamily.Application.SpeciesManagement.Queries.GetBreedsByIdWithPagination;

namespace PetFamily.API.Requests.Species;

public record GetBreedsByIdWithPaginationRequest(int Page, int PageSize)
{
    public GetBreedsByIdWithPaginationQuery ToQuery(Guid speciesId) => new(speciesId, Page, PageSize);
}