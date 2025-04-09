namespace PetFamily.Accounts.Contracts.Requests;

public record ApproveRequestRequest(
    Guid UserId,
    string PhoneNumber,
    int Experience,
    string Description);