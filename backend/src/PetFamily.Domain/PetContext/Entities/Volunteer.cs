using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.SharedVO;

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
    
    public TransferDetails TransferDetails { get; private set; }
    
    private readonly List<Pet> _allOwnedPets;

    public AllOwnedPets AllOwnedPets
    {
        get
        {
            var petsCreatResult =  AllOwnedPets.Create(_allOwnedPets);
            return petsCreatResult.Value;
        }
        private set {}
    }

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
        TransferDetails transferDetails,
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
        TransferDetails = transferDetails;
        _allOwnedPets = allOwnedPets;
    }

    public static Result<Volunteer> Create(
        VolunteerId id,
        VolunteerFio fio,
        Phone phoneNumber,
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        SocialWeb socialWeb,
        TransferDetails transferDetails,
        List<Pet> allOwnedPets)
    {
        var fioCreateResult = VolunteerFio.Create(fio.FirstName, fio.LastName, fio.Surname);
        if (fioCreateResult.IsFailure)
            return Result.Failure<Volunteer>(fioCreateResult.Error);

        var phoneCreateResult = Phone.Create(phoneNumber.Number);
        if (phoneCreateResult.IsFailure)
            return Result.Failure<Volunteer>(phoneCreateResult.Error);
        
        var emailCreateResult = Email.Create(email.Value);
        if (emailCreateResult.IsFailure)
            return Result.Failure<Volunteer>(emailCreateResult.Error);
                
        var descriptionCreateResult = Description.Create(description.Value);
        if (descriptionCreateResult.IsFailure)
            return Result.Failure<Volunteer>(descriptionCreateResult.Error);
        
        var yearsOfExperienceCreateResult = YearsOfExperience.Create(yearsOfExperience.Value);
        if (yearsOfExperienceCreateResult.IsFailure)
            return Result.Failure<Volunteer>(yearsOfExperienceCreateResult.Error);
        
        var socialWebCreateResult = SocialWeb.Create(socialWeb.Name, socialWeb.Link);
        if (socialWebCreateResult.IsFailure)
            return Result.Failure<Volunteer>(socialWebCreateResult.Error);

        var transferDetailsCreateResult = TransferDetails.Create(transferDetails.Name, transferDetails.Description);
        if (transferDetailsCreateResult.IsFailure)
            return Result.Failure<Volunteer>(transferDetailsCreateResult.Error);

        var volunteer = new Volunteer(
            id,
            fioCreateResult.Value,
            phoneCreateResult.Value,
            emailCreateResult.Value,
            descriptionCreateResult.Value,
            yearsOfExperienceCreateResult.Value,
            socialWebCreateResult.Value,
            transferDetailsCreateResult.Value, 
            allOwnedPets);
        
        return Result.Success(volunteer);
    }

    private int CountPetsWithHome() => AllOwnedPets.Value.Count(p => p.HelpStatus == HelpStatus.FindHome);

    private int CountPetsTryFindHome() => AllOwnedPets.Value.Count(p => p.HelpStatus == HelpStatus.SeekHome); 
    
    private int CountPetsUnderTreatment() => AllOwnedPets.Value.Count(p => p.HelpStatus == HelpStatus.NeedHelp); 
}
