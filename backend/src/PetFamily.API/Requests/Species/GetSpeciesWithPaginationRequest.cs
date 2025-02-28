using PetFamily.Application.Abstractions;
using PetFamily.Application.SpeciesManagement.Queries;
using PetFamily.Application.SpeciesManagement.Queries.GetSpeciesWithPagination;

namespace PetFamily.API.Requests.Species;

public record GetSpeciesWithPaginationRequest(int Page, int PageSize)
{
    public GetSpeciesWithPaginationQuery ToQuery() => new(Page, PageSize); 
}