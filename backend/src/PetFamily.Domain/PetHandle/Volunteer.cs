using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetHandle;

public class Volunteer
{
    public Guid Id { get; private set; }
    
    public string FIO { get; private set; }
    
    public string Email { get; private set; }
    
    public string Description { get; private set; }
    
    public int YearsOfExperience { get; private set; }

    public int SumPetsWithHome {get; private set;}
    
    public int SumPetsTryFindHome {get; private set;}
    
    public int SumPetsUnderTreatment {get; private set;}
    
    public string PhoneNumber { get; private set; }
    
    public SocialWeb SocialWeb { get; private set; }
    
    public TransferDetails TransferDetails { get; private set; }
    
    private readonly List<Pet> _allOwnedPets;
    public IReadOnlyList<Pet> AllOwnedPets => _allOwnedPets;

    private Volunteer(
        string fio,
        string email,
        string description,
        int yearsOfExperience,
        string phoneNumber,
        SocialWeb socialWeb,
        TransferDetails transferDetails,
        List<Pet> allOwnedPets
    )
    {
        FIO = fio;
        Email = email;
        Description = description;
        YearsOfExperience = yearsOfExperience;
        SumPetsWithHome = CountPetsWithHome();
        SumPetsTryFindHome = CountPetsTryFindHome();
        SumPetsUnderTreatment = CountPetsUnderTreatment();
        PhoneNumber = phoneNumber;
        SocialWeb = socialWeb;
        TransferDetails = transferDetails;
        _allOwnedPets = allOwnedPets;
    }

    public static Result<Volunteer> Create(
        string fio,
        string email,
        string description,
        int yearsOfExperience,
        string phoneNumber,
        SocialWeb socialWeb,
        TransferDetails transferDetails,
        List<Pet> allOwnedPets)
    {
        if (string.IsNullOrWhiteSpace(fio))
            return Result.Failure<Volunteer>("Fio cannot not be null or empty");
        
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<Volunteer>("Email cannot not be null or empty");
                
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Volunteer>("Description cannot not be null or empty");
        
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return Result.Failure<Volunteer>("PhoneNumber cannot not be null or empty");

        var volunteer = new Volunteer(
            fio,
            email,
            description,
            yearsOfExperience,
            phoneNumber,
            socialWeb,
            transferDetails, 
            allOwnedPets);
        
        return Result.Success(volunteer);
    }
    
    private int CountPetsWithHome() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.FindHome);

    private int CountPetsTryFindHome() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.SeekHome); 
    
    private int CountPetsUnderTreatment() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.NeedHelp); 
}
