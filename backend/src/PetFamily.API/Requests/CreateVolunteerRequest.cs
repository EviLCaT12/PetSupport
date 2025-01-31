using FluentValidation;
using PetFamily.Application.DTO.Shared;
using PetFamily.Application.Dto.Volunteer;

namespace PetFamily.API.Requests;

public record CreateVolunteerRequest(
    VolunteerDto VolunteerDto,
    List<SocialWebDto> SocialWebDto,
    List<TransferDetailDto> TransferDetailDto
    );
    