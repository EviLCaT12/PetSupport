using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Domain.PetContext.Entities;

public class Volunteer : Entity<VolunteerId>
{
    private bool _isDeleted = false;
    public VolunteerId Id { get; private set; }
    
    public VolunteerFio Fio { get; private set; }
    
    public Phone Phone { get; private set; }
    
    public Email Email { get; private set; }
    public Description Description { get; private set; }
    
    public YearsOfExperience YearsOfExperience { get; private set; }

    public int SumPetsWithHome {get; private set;}
    
    public int SumPetsTryFindHome {get; private set;}
    
    public int SumPetsUnderTreatment {get; private set;}
    
    public SocialWebList SocialWeb { get; private set; }
    
    public TransferDetailsList TransferDetailsList { get; private set; }

    private readonly List<Pet> _pets = []; //создатся отдельный метод Add, который и будет добавлять животных
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
        SocialWebList socialWebs,
        TransferDetailsList transferDetails
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
        SocialWeb = socialWebs;
        TransferDetailsList = transferDetails;
    }

    public static Result<Volunteer, Error> Create(
        VolunteerId id,
        VolunteerFio fio,
        Phone phoneNumber,
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        SocialWebList socialWebsList,
        TransferDetailsList transferDetailsList)
    {
        var volunteer = new Volunteer(
            id,
            fio,
            phoneNumber,
            email,
            description,
            yearsOfExperience,
            socialWebsList,
            transferDetailsList);
        
        return volunteer;
    }

    public void UpdateMainInfo(
        VolunteerFio newFio,
        Phone newPhone,
        Email newEmail,
        Description newDescription,
        YearsOfExperience newYearsOfExperience
    )
    {
        Fio = newFio;
        Phone = newPhone;
        Email = newEmail;
        Description = newDescription;
        YearsOfExperience = newYearsOfExperience;
    }

    public void UpdateSocialWebList(IEnumerable<SocialWeb> newSocialWebs)
    {
        SocialWeb = SocialWebList.Create(newSocialWebs).Value; 
    }
    
    public void UpdateTransferDetailsList(IEnumerable<TransferDetails> newTransferDetails)
    {
        TransferDetailsList = TransferDetailsList.Create(newTransferDetails).Value;
    }

    public void Delete()
    {
        _isDeleted = true;
        foreach (var pet in _pets)
        {
            pet.Delete();
        }
    }
    
    public void Restore()
    {
        _isDeleted = false;
        foreach (var pet in _pets)
        {
            pet.Restore();
        }
    }

    private int CountPetsWithHome() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.FindHome);

    private int CountPetsTryFindHome() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.SeekHome); 
    
    private int CountPetsUnderTreatment() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.NeedHelp); 
}
