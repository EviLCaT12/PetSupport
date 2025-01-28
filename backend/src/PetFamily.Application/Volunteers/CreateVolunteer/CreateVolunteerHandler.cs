using CSharpFunctionalExtensions;
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
        CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.NewVolunteerId(); 
        
        var fioCreateResult = VolunteerFio.Create(request.firstName, request.lastName, request.surName);
        if (fioCreateResult.IsFailure)
            return fioCreateResult.Error;

        var phoneNumberCreateResult = Phone.Create(request.phoneNumber);
        if (phoneNumberCreateResult.IsFailure)
            return phoneNumberCreateResult.Error;
        
        var emailCreateResult = Email.Create(request.email);
        if (emailCreateResult.IsFailure)
            return emailCreateResult.Error;
        
        var descriptionCreateResult = Description.Create(request.description);
        if (descriptionCreateResult.IsFailure)
            return descriptionCreateResult.Error;
        
        var yOExpCreateResult = YearsOfExperience.Create(request.yearsOfExperience);
        if (yOExpCreateResult.IsFailure)
            return yOExpCreateResult.Error;
        
        var socialWebCreateResult = SocialWeb.Create(request.link, request.socialWebName);
        if (socialWebCreateResult.IsFailure)
            return socialWebCreateResult.Error;
        
        List<TransferDetails> transferDetails = [];
        foreach (var transferDetail in request.transferDetails)
        {
            var transferDetailCreateResult = TransferDetails.Create(transferDetail.Item1, transferDetail.Item2);
            if (transferDetailCreateResult.IsFailure)
                return transferDetailCreateResult.Error;

            transferDetails.Add(transferDetailCreateResult.Value); 
        }

        var allOwnedPets = request.allOwnedPets;
        
        var validVolunteer = Volunteer.Create(
            volunteerId,
            fioCreateResult.Value,
            phoneNumberCreateResult.Value,
            emailCreateResult.Value,
            descriptionCreateResult.Value,
            yOExpCreateResult.Value,
            socialWebCreateResult.Value,
            transferDetails,
            allOwnedPets);
        
        await _volunteerRepository.AddAsync(validVolunteer.Value, cancellationToken);
        
        return validVolunteer.Value.Id.Value;
    }
}