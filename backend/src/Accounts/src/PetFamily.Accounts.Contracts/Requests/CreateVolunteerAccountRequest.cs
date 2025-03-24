using PetFamily.Core.Dto.VolunteerDto;

namespace PetFamily.Accounts.Contracts.Requests;

public record CreateVolunteerAccountRequest(
    string Email, 
    string UserName,
    FioDto Fio,
    string Password,
    int Experience);