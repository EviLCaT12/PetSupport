using PetFamily.Volunteers.Contracts.Dto.VolunteerDto;

namespace PetFamily.Volunteers.Contracts.Requests.Volunteer.UpdateVolunteer;

public record UpdateVolunteerMainInfoRequest(
    FioDto Fio,
    string Phone,
    string Email,
    string Description,
    int YearsOfExperience);