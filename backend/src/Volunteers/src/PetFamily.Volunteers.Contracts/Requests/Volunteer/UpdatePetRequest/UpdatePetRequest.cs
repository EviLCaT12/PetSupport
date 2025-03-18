using PetFamily.Core.Dto.PetDto;
using PetFamily.Core.Dto.Shared;

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
