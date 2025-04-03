using PetFamily.Core.Dto.Shared;
using PetFamily.Volunteers.Contracts.Dto.PetDto;

namespace PetFamily.Volunteers.Contracts.Requests.Volunteer.UpdatePetRequest;

public record UpdatePetRequest(
    string? Name,
    PetClassificationDto Classification,
    string? Description,
    string? Color,
    string? HealthInfo,
    AddressDto? Address,
    DimensionsDto? Dimensions,
    string? OwnerPhone,
    bool? IsCastrate,
    DateTime? DateOfBirth,
    bool? IsVaccinated,
    int? HelpStatus,
    IEnumerable<TransferDetailDto>? TransferDetailsDto);
