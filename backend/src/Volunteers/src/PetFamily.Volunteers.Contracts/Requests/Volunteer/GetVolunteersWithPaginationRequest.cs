namespace PetFamily.Volunteers.Contracts.Requests.Volunteer;

//Как будто бы можно использовать 1 реквест на все сущности, для которых требуется постраничный вывод
public record GetVolunteersWithPaginationRequest(int Page, int PageSize);