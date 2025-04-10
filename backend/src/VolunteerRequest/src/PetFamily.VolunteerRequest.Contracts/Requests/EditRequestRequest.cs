using PetFamily.Core.Dto.Shared;

namespace PetFamily.VolunteerRequest.Contracts.Requests;

public record EditRequestRequest(
    FioDto Fio,
    string Description,
    string Email,
    int Experience);