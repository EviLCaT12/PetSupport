using PetFamily.Application.Dto.VolunteerDto;
using PetFamily.Application.Volunteers.UpdateMainInfo;

namespace PetFamily.API.Requests;

public record UpdateVolunteerRequest(
    FioDto Fio, 
    string Phone,
    string Email,
    string Description,
    int YearsOfExperience);