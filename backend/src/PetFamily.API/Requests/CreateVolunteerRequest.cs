using FluentValidation;
using PetFamily.Application.Dto.Shared;
using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.API.Requests;

public record CreateVolunteerRequest(
    CreateVolunteerCommand CreateVolunteerCommand,
    List<SocialWebDto> SocialWebDto,
    List<TransferDetailDto> TransferDetailDto
    );
    