using PetFamily.Application.PetManagement.Queries.GetVolunteersWithPagination;

namespace PetFamily.API.Requests.Shared;

//Как будто бы можно использовать 1 реквест на все сущности, для которых требуется постраничный вывод
public record GetEntityWithPaginationRequest(int Page, int PageSize)
{
    public GetVolunteerWithPaginationQuery ToQuery() =>
        new(Page, PageSize);
}