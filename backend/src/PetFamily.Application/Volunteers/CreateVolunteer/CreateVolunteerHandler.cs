using CSharpFunctionalExtensions;
using PetFamily.Application.DTO.Shared;
using PetFamily.Application.Dto.Volunteer;
using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteerRepository;

    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteerRepository = volunteersRepository;
    }
    
    public async Task<Result<Guid, Error>> HandleAsync(
        VolunteerDto volunteerDto,
        List<SocialWebDto> socialWebDto,
        List<TransferDetailDto> transferDetailDto,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.NewVolunteerId(); 
        
        var fioCreateResult = VolunteerFio.Create(volunteerDto.FirstName, volunteerDto.LastName, volunteerDto.SurName);
        if (fioCreateResult.IsFailure)
            return fioCreateResult.Error;

        var phoneNumberCreateResult = Phone.Create(volunteerDto.PhoneNumber);
        if (phoneNumberCreateResult.IsFailure)
            return phoneNumberCreateResult.Error;
        
        var emailCreateResult = Email.Create(volunteerDto.Email);
        if (emailCreateResult.IsFailure)
            return emailCreateResult.Error;
        
        var descriptionCreateResult = Description.Create(volunteerDto.Description);
        if (descriptionCreateResult.IsFailure)
            return descriptionCreateResult.Error;
        
        var yOExpCreateResult = YearsOfExperience.Create(volunteerDto.YearsOfExperience);
        if (yOExpCreateResult.IsFailure)
            return yOExpCreateResult.Error;
        
        List<SocialWeb> socialWebs = [];
        foreach (var socialWebCreateResult in socialWebDto
                     .Select(socialWeb => SocialWeb.Create(socialWeb.Link, socialWeb.Name)))
        {
            if (socialWebCreateResult.IsFailure)
                return socialWebCreateResult.Error;

            socialWebs.Add(socialWebCreateResult.Value);
        };
        var socialWebList = SocialWebList.Create(socialWebs);
        
        List<TransferDetails> transferDetails = [];
        foreach (var transferDetailCreateResult in transferDetailDto
                     .Select(transferDetail => TransferDetails.Create(transferDetail.Name, transferDetail.Description)))
        {
            if (transferDetailCreateResult.IsFailure)
                return transferDetailCreateResult.Error;

            transferDetails.Add(transferDetailCreateResult.Value);
        }
        var transferDetailsList = TransferDetailsList.Create(transferDetails);
        
        var validVolunteer = Volunteer.Create(
            volunteerId,
            fioCreateResult.Value,
            phoneNumberCreateResult.Value,
            emailCreateResult.Value,
            descriptionCreateResult.Value,
            yOExpCreateResult.Value,
            socialWebList.Value,
            transferDetailsList.Value
            );
        
        await _volunteerRepository.AddAsync(validVolunteer.Value, cancellationToken);
        
        return validVolunteer.Value.Id.Value;
    }
}