using PetFamily.Application.Dto.VolunteerDto;

namespace PetFamily.API.Requests.UpdateVolunteer;

public record UpdateVolunteerMainInfoRequest(
    FioDto Fio, 
    string Phone,
    string Email,
    string Description,
    int YearsOfExperience);