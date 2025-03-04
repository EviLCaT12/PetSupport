using PetFamily.Application.Dto.PetDto;
using PetFamily.Application.Dto.Shared;
using PetFamily.Application.PetManagement.Commands.AddPet;

namespace PetFamily.API.Requests.Volunteers.AddPet;

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
    string HelpStatus,
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