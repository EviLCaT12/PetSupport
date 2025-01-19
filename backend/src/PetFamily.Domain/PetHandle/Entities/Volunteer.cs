using CSharpFunctionalExtensions;
using PetFamily.Domain.PetHandle.ValueObjects;

namespace PetFamily.Domain.PetHandle.Entities;

public class Volunteer : Entity
{
    public VolunteerId Id { get; private set; }
    
    public VolunteerFio FIO { get; private set; }
    
    public VolunteerContact Contacts { get; private set; }
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
        VolunteerContact contacts,
        string description,
        int yearsOfExperience,
        SocialWeb socialWeb,
        TransferDetails transferDetails,
        List<Pet> allOwnedPets
    )
    {
        Id = id;
        FIO = fio;
        Contacts = contacts;
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
        VolunteerContact contacts,
        string description,
        int yearsOfExperience,
        SocialWeb socialWeb,
        TransferDetails transferDetails,
        List<Pet> allOwnedPets)
    {
        var fioCreateResult = VolunteerFio.Create(fio.FirstName, fio.LastName, fio.Surname);
        if (fioCreateResult.IsFailure)
            return Result.Failure<Volunteer>(fioCreateResult.Error);

        var contactsCreateResult = VolunteerContact.Create(contacts.Email, contacts.PhoneNumber);
        if (contactsCreateResult.IsFailure)
            return Result.Failure<Volunteer>(contactsCreateResult.Error);
                
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Volunteer>("Description cannot not be null or empty");

        var volunteer = new Volunteer(
            id,
            fioCreateResult.Value,
            contactsCreateResult.Value,
            description,
            yearsOfExperience,
            socialWeb,
            transferDetails, 
            allOwnedPets);
        
        return Result.Success(volunteer);
    }
    
    private int CountPetsWithHome() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.FindHome);

    private int CountPetsTryFindHome() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.SeekHome); 
    
    private int CountPetsUnderTreatment() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.NeedHelp); 
}
