namespace PetFamily.VolunteerRequest.Contracts.Requests;

public record GetRequestsForCurrentAdminRequest(
    string Status,
    int Page,
    int PageSize);