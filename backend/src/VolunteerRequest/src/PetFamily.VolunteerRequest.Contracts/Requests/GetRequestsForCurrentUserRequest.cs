namespace PetFamily.VolunteerRequest.Contracts.Requests;

public record GetRequestsForCurrentUserRequest(   
    string Status,
    int Page,
    int PageSize);