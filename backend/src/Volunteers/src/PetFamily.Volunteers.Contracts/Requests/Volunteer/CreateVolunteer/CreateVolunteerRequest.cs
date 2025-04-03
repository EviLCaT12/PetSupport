using PetFamily.Core.Dto.Shared;
using PetFamily.Volunteers.Contracts.Dto.VolunteerDto;

namespace PetFamily.Volunteers.Contracts.Requests.Volunteer.CreateVolunteer;

public record CreateVolunteerRequest(
    FioDto Fio,
    string PhoneNumber,
    string Email,
    string Description
);
    