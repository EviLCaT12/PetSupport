namespace PetFamily.Accounts.Contracts.Requests;

public record CreateVolunteerAccountRequest(
    string Email, 
    string UserName,
    string Password,
    int Experience);