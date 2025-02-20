using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;
using PetFamily.Domain.Shared;
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
    
    private List<SocialWeb> _socialWebs = [];

    public IReadOnlyList<SocialWeb> SocialWebList
    {
        get => _socialWebs;
        private set => _socialWebs = value.ToList();
    }
    
    private List<TransferDetails> _transferDetails = [];

    public IReadOnlyList<TransferDetails> TransferDetailsList
    {
        get => _transferDetails;
        private set => _transferDetails = value.ToList();
    }

    private readonly List<Pet> _pets = [];
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
        IEnumerable<SocialWeb> socialWebsList,
        IEnumerable<TransferDetails> transferDetails
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
        _socialWebs = socialWebsList.ToList();
        _transferDetails = transferDetails.ToList();
    }

    public static Result<Volunteer, Error> Create(
        VolunteerId id,
        VolunteerFio fio,
        Phone phoneNumber,
        Email email,
        Description description,
        YearsOfExperience yearsOfExperience,
        IEnumerable<SocialWeb> socialWebsList,
        IEnumerable<TransferDetails> transferDetailsList)
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
        => _socialWebs = newSocialWebs.ToList();
    
    public void UpdateTransferDetailsList(IEnumerable<TransferDetails> newTransferDetails)
        => _transferDetails = newTransferDetails.ToList();

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

    public UnitResult<ErrorList> AddPet(Pet pet)
    {
        var position = Position.Create(_pets.Count + 1);
        if (position.IsFailure)
        {
            var error = position.Error;
            return new ErrorList([error]);
        }
        
        pet.SetPosition(position.Value);
        _pets.Add(pet);

        return Result.Success<ErrorList>();
    }

    public void AddPetPhotos(PetId petId, IEnumerable<PetPhoto> photos)
    {
        var pet = AllOwnedPets.FirstOrDefault(p => p.Id == petId)!;
        pet.AddPhotos(photos);
        
    }
    
    public UnitResult<ErrorList> DeletePetPhotos(PetId petId, IEnumerable<PetPhoto> photos)
    {
        var pet = AllOwnedPets.FirstOrDefault(p => p.Id == petId)!;
        var removeResult = pet.DeletePhotos(photos);
        if (removeResult.IsFailure)
            return removeResult.Error;
        
        return Result.Success<ErrorList>();
    }
    
    public UnitResult<ErrorList> MovePetToSpecifiedPosition(PetId petId, Position newPosition)
    {
        var currentPosition = _pets
            .FirstOrDefault(p => p.Id == petId)!.Position; 

        if (currentPosition == newPosition || _pets.Count == 1)
            return Result.Success<ErrorList>();

        var adjustedPosition = AdjustNewPositionIfOutOfRange(newPosition);
        
        if (adjustedPosition.IsFailure)
            return adjustedPosition.Error;
        
        newPosition = adjustedPosition.Value;

        var moveResult = MovePetsBetweenPosition(newPosition, currentPosition);
        if (moveResult.IsFailure)
            return moveResult.Error;
        
        return Result.Success<ErrorList>();
    }

    public UnitResult<ErrorList> MovePetToFirstPosition(PetId petId)
    {
        var currentPet = _pets.FirstOrDefault(p => p.Id == petId);
        
        var currentPosition = currentPet!.Position;
        
        if (currentPosition.Value == 1)
            return Result.Success<ErrorList>();
        
        var petsToMove = _pets.Where(p => p.Position.Value < currentPosition.Value);

        var firstPet = petsToMove.First().Position.Value;
        
        foreach (var pet in petsToMove)
        {
            var result = pet.MoveForward(firstPet, _pets.Count);
            if (result.IsFailure)
                return result.Error;
        }

        var firstPosition = Position.Create(1).Value;
        
        currentPet.SetPosition(firstPosition);
        
        return Result.Success<ErrorList>();
    }
    
    public UnitResult<ErrorList> MovePetToLastPosition(PetId petId)
    {
        var currentPet = _pets.FirstOrDefault(p => p.Id == petId);
        
        var currentPosition = currentPet!.Position;
        
        if (currentPosition.Value == _pets.Count)
            return Result.Success<ErrorList>();
        
        var petsToMove = _pets.Where(p => p.Position.Value > currentPosition.Value);
        
        var firstPet = petsToMove.First().Position.Value;
        
        foreach (var pet in petsToMove)
        {
            var result = pet.MoveBackward(firstPet, _pets.Count);
            if (result.IsFailure)
                return result.Error;
        }

        var lastPosition = Position.Create(_pets.Count).Value;
        
        currentPet.SetPosition(lastPosition);
        
        return Result.Success<ErrorList>();
    }

    private UnitResult<ErrorList> MovePetsBetweenPosition(Position newPosition, Position currentPosition)
    {
        if (newPosition.Value < currentPosition.Value)
        {
            var petToMove = _pets.Where(p => p.Position.Value >= newPosition.Value
                                             && p.Position.Value <= currentPosition.Value).ToList();

            var firstElement = petToMove.Min(p => p.Position.Value);
            var lastElement = petToMove.Max(p => p.Position.Value);
            foreach (var pet in petToMove)
            {
                var result = pet.MoveForward(firstElement, lastElement);
                if (result.IsFailure)
                    return result.Error;
            }
        }
        else if (newPosition.Value > currentPosition.Value)
        {
            var petToMove = _pets
                .Where(p => p.Position.Value <= newPosition.Value
                                             && p.Position.Value >= currentPosition.Value).ToList();

            var firstElement = petToMove.Min(p => p.Position.Value);
            var lastElement = petToMove.Max(p => p.Position.Value);
            
            foreach (var pet in petToMove)
            {
                var result = pet.MoveBackward(firstElement,lastElement);
                if (result.IsFailure)
                    return result.Error;
            }

        }
        return Result.Success<ErrorList>();
    }

    private Result<Position, ErrorList> AdjustNewPositionIfOutOfRange(Position newPosition)
    {
        if (newPosition.Value <= _pets.Count)
            return newPosition;
        
        var lastPosition = Position.Create(_pets.Count);
        if (lastPosition.IsFailure)
        {
            var error = lastPosition.Error;
            return new ErrorList([error]);
        }

        return lastPosition.Value;
    }

    private int CountPetsWithHome() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.FindHome);

    private int CountPetsTryFindHome() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.SeekHome); 
    
    private int CountPetsUnderTreatment() => AllOwnedPets.Count(p => p.HelpStatus == HelpStatus.NeedHelp); 
}
