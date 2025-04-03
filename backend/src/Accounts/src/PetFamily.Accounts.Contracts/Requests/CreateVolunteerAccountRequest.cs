namespace PetFamily.Accounts.Contracts.Requests;

public record CreateVolunteerAccountRequest(
    string Email, 
    int Experience,
    string PhoneNumber,
    string Description);