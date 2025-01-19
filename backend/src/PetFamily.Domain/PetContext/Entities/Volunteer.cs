using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext.ValueObjects;

namespace PetFamily.Domain.PetContext.Entities;

public class Volunteer : Entity
{
    public VolunteerId Id { get; private set; }
    
    public VolunteerFio FIO { get; private set; }
    
    public Phone Phone { get; private set; }
    
    public Email Email { get; private set; }
    public string Description { get; private set; }
    
    public int YearsOfExperience { get; private set; }

    public int SumPetsWithHome {get; private set;}
    
    public int SumPetsTryFindHome {get; private set;}
    
    public int SumPetsUnderTreatment {get; private set;}
    
    public SocialWeb SocialWeb { get; private set; }
    
    public TransferDetails TransferDetails { get; private set; }
    
    private readonly List<Pet> _allOwnedPets;
    public IReadOnlyList<Pet> AllOwnedPets => _allOwnedPets;

    private Volunteer(
        VolunteerId id,
        VolunteerFio fio,
        Phone phoneNumber,
        Email email,
        string description,
        int yearsOfExperience,
        SocialWeb socialWeb,
        TransferDetails transferDetails,
        List<Pet> allOwnedPets
    )
    {
        Id = id;
        FIO = fio;
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
        string description,
        int yearsOfExperience,
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
                
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Volunteer>("Description cannot not be null or empty");
        
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
            description,
            yearsOfExperience,
            socialWebCreateResult.Value,
            transferDetailsCreateResult.Value, 
            allOwnedPets);
        
        return Result.Success(volunteer);
    }
    
    private int CountPetsWithHome() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.FindHome);

    private int CountPetsTryFindHome() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.SeekHome); 
    
    private int CountPetsUnderTreatment() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.NeedHelp); 
}
