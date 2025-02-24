namespace PetFamily.Application.PetManagement.Queries.GetPetsWithPagination;

public record GetPetsWithPaginationQuery(
    int Page,
    int PageSize);