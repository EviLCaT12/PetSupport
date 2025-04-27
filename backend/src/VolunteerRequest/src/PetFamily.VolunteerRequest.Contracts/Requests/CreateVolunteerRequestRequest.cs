using PetFamily.Core.Dto.Shared;

namespace PetFamily.VolunteerRequest.Contracts.Requests;

public record CreateVolunteerRequestRequest(
    FioDto FullName,
    string Description,
    string Email,
    int Experience);