using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Dto.Shared;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler(
    IVolunteersRepository volunteersRepository,
    IValidator<CreateVolunteerCommand> createVolunteerCommandValidator,
    IValidator<IEnumerable<SocialWebDto>> socialWebDtoValidator,
    IValidator<IEnumerable<TransferDetailDto>> transferDetailDtoValidator)
{
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        CreateVolunteerCommand createVolunteerCommand,
        List<SocialWebDto> socialWebDto,
        List<TransferDetailDto> transferDetailDto,
        CancellationToken cancellationToken = default)
    {
        var createVolunteerValidationResult = await createVolunteerCommandValidator.ValidateAsync(
            createVolunteerCommand,
            cancellationToken);
        if (createVolunteerValidationResult.IsValid == false)
            return createVolunteerValidationResult.ToErrorList();
        
        var socialWebDtoValidatorResult = await socialWebDtoValidator.ValidateAsync(socialWebDto, cancellationToken);
        if (socialWebDtoValidatorResult.IsValid == false)
            return socialWebDtoValidatorResult.ToErrorList();
        
        var transferDetailDtoValidatorResult = await transferDetailDtoValidator.ValidateAsync(transferDetailDto, cancellationToken);
        if (transferDetailDtoValidatorResult.IsValid == false)
            return transferDetailDtoValidatorResult.ToErrorList();
        
        
        var volunteerId = VolunteerId.NewVolunteerId();

        var fioCreateResult = VolunteerFio
            .Create(createVolunteerCommand.FirstName, createVolunteerCommand.LastName, createVolunteerCommand.SurName)
            .Value;
        

        var phoneNumberCreateResult = Phone.Create(createVolunteerCommand.PhoneNumber).Value;



        var emailCreateResult = Email.Create(createVolunteerCommand.Email).Value;

        var descriptionCreateResult = Description.Create(createVolunteerCommand.Description).Value;
        
        var yOExpCreateResult = YearsOfExperience.Create(createVolunteerCommand.YearsOfExperience).Value;
        
        List<SocialWeb> socialWebs = [];
        socialWebs.AddRange(socialWebDto
            .Select(socialWeb => SocialWeb.Create(socialWeb.Link, socialWeb.Name))
            .Select(socialWebCreateResult => socialWebCreateResult.Value));
        var socialWebList = SocialWebList.Create(socialWebs);
        
        List<TransferDetails> transferDetails = [];
        transferDetails.AddRange(transferDetailDto
            .Select(transferDetail => TransferDetails.Create(transferDetail.Name, transferDetail.Description))
            .Select(transferDetailsCreateResult => transferDetailsCreateResult.Value));
        var transferDetailsList = TransferDetailsList.Create(transferDetails);
        
        var validVolunteer = Volunteer.Create(
            volunteerId,
            fioCreateResult,
            phoneNumberCreateResult,
            emailCreateResult,
            descriptionCreateResult,
            yOExpCreateResult,
            socialWebList.Value,
            transferDetailsList.Value
            );
        
        await volunteersRepository.AddAsync(validVolunteer.Value, cancellationToken);
        
        return validVolunteer.Value.Id.Value;
    }
}