using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Domain.PetContext.Entities;

public class Volunteer : Entity<VolunteerId>
{
    public VolunteerId Id { get; private set; }
    
    public VolunteerFio Fio { get; private set; }
    
    public Phone Phone { get; private set; }
    
    public Email Email { get; private set; }
    public Description Description { get; private set; }
    
    public YearsOfExperience YearsOfExperience { get; private set; }

    public int SumPetsWithHome {get; private set;}
    
    public int SumPetsTryFindHome {get; private set;}
    
    public int SumPetsUnderTreatment {get; private set;}
    
    public SocialWeb SocialWeb { get; private set; }
    
    public TransferDetailsList TransferDetailsList { get; private set; }
    
    private readonly List<Pet> _pets;
    public IReadOnlyList<Pet> AllOwnedPets => _pets;

    // ef core
    private Volunteer() {}

    private Volunteer(
        VolunteerId id,
        VolunteerFio fio,
        Phone phoneNumber,
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        SocialWeb socialWeb,
        TransferDetailsList transferDetails,
        List<Pet> allOwnedPets
    )
    {
        Id = id;
        Fio = fio;
        Phone = phoneNumber;
        Email = email;
        Description = description;
        YearsOfExperience = yearsOfExperience;
        SumPetsWithHome = CountPetsWithHome();
        SumPetsTryFindHome = CountPetsTryFindHome();
        SumPetsUnderTreatment = CountPetsUnderTreatment();
        SocialWeb = socialWeb;
        TransferDetailsList = transferDetails;
        _pets = allOwnedPets;
    }

    public static Result<Volunteer, Error> Create(
        VolunteerId id,
        VolunteerFio fio,
        Phone phoneNumber,
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        SocialWeb socialWeb,
        List<TransferDetails> transferDetails,
        List<Pet> allOwnedPets)
    {
        var fioCreateResult = VolunteerFio.Create(fio.FirstName, fio.LastName, fio.Surname);
        if (fioCreateResult.IsFailure)
            return fioCreateResult.Error;

        var phoneCreateResult = Phone.Create(phoneNumber.Number);
        if (phoneCreateResult.IsFailure)
            return phoneCreateResult.Error;
        
        var emailCreateResult = Email.Create(email.Value);
        if (emailCreateResult.IsFailure)
            return emailCreateResult.Error;
                
        var descriptionCreateResult = Description.Create(description.Value);
        if (descriptionCreateResult.IsFailure)
            return descriptionCreateResult.Error;
        
        var yearsOfExperienceCreateResult = YearsOfExperience.Create(yearsOfExperience.Value);
        if (yearsOfExperienceCreateResult.IsFailure)
            return yearsOfExperienceCreateResult.Error;
        
        var socialWebCreateResult = SocialWeb.Create(socialWeb.Name, socialWeb.Link);
        if (socialWebCreateResult.IsFailure)
            return socialWebCreateResult.Error;

        var transferDetailsListCreateResult = TransferDetailsList.Create(transferDetails);
        if(transferDetailsListCreateResult.IsFailure)
            return transferDetailsListCreateResult.Error;
            

        var volunteer = new Volunteer(
            id,
            fioCreateResult.Value,
            phoneCreateResult.Value,
            emailCreateResult.Value,
            descriptionCreateResult.Value,
            yearsOfExperienceCreateResult.Value,
            socialWebCreateResult.Value,
            transferDetailsListCreateResult.Value, 
            allOwnedPets);
        
        return volunteer;
    }

    private int CountPetsWithHome() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.FindHome);

    private int CountPetsTryFindHome() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.SeekHome); 
    
    private int CountPetsUnderTreatment() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.NeedHelp); 
}
