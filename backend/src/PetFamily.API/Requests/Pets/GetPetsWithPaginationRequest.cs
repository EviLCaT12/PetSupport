using PetFamily.Application.PetManagement.Queries.GetPetsWithPagination;

namespace PetFamily.API.Requests.Pets;

public record GetPetsWithPaginationRequest(int Page, int PageSize)
{
    public GetPetsWithPaginationQuery ToQuery() =>
        new (Page, PageSize);
}