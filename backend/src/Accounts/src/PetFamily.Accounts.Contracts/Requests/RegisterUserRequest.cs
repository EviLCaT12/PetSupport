using PetFamily.Volunteers.Contracts.Dto.VolunteerDto;

namespace PetFamily.Accounts.Contracts.Requests;

public record RegisterUserRequest(
    string Email, 
    string UserName,
    FioDto Fio,
    string Password);