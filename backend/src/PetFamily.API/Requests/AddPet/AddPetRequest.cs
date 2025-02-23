using PetFamily.Application.Dto.PetDto;
using PetFamily.Application.Dto.Shared;
using PetFamily.Application.Volunteers.AddPet;

namespace PetFamily.API.Requests.AddPet;

public record AddPetRequest(
    string Name,
    PetClassificationDto Classification,
    string Description,
    string Color,
    string HealthInfo,
    AddressDto Address,
    DimensionsDto Dimensions,
    string OwnerPhone,
    bool IsCastrate,
    DateTime DateOfBirth,
    bool IsVaccinated,
    int HelpStatus,
    IEnumerable<TransferDetailDto> TransferDetailsDto)
{
    public AddPetCommand ToCommand(Guid volunteerId)
        => new AddPetCommand(
            volunteerId,
            Name,
            Classification,
            Description,
            Color,
            HealthInfo,
            Address,
            Dimensions,
            OwnerPhone,
            IsCastrate,
            DateOfBirth,
            IsVaccinated,
            HelpStatus,
            TransferDetailsDto);
}