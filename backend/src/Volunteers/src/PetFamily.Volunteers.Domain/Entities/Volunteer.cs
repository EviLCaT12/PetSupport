using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Volunteers.Domain.ValueObjects.PetVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Domain.Entities;

public class Volunteer : Entity<VolunteerId>
{
    private bool _isDeleted = false;
    
    public bool IsDeleted => _isDeleted;
    
    public VolunteerId Id { get; private set; }
    
    public Fio Fio { get; private set; }
    
    public Phone Phone { get; private set; }
    
    public Email Email { get; private set; }
    public Description Description { get; private set; }
    
    public int SumPetsWithHome {get; private set;}
    
    public int SumPetsTryFindHome {get; private set;}
    
    public int SumPetsUnderTreatment {get; private set;}

    private readonly List<Pet> _pets = [];
    public IReadOnlyList<Pet> AllOwnedPets => _pets;

    // ef core
    private Volunteer() {}

    private Volunteer(
        VolunteerId id,
        Fio fio,
        Phone phoneNumber,
        Email email,
        Description description
    )
    {
        Id = id;
        Fio = fio;
        Phone = phoneNumber;
        Email = email;
        Description = description;
        SumPetsWithHome = CountPetsWithHome();
        SumPetsTryFindHome = CountPetsTryFindHome();
        SumPetsUnderTreatment = CountPetsUnderTreatment();
    }

    public static Result<Volunteer, Error> Create(
        VolunteerId id,
        Fio fio,
        Phone phoneNumber,
        Email email,
        Description description)
    {
        var volunteer = new Volunteer(
            id,
            fio,
            phoneNumber,
            email,
            description);
        
        return volunteer;
    }

    public void UpdateMainInfo(
        Fio newFio,
        Phone newPhone,
        Email newEmail,
        Description newDescription
    )
    {
        Fio = newFio;
        Phone = newPhone;
        Email = newEmail;
        Description = newDescription;
    }

    public void UpdatePet(
        Pet updatedPet,
        Name? name,
        PetClassification classification,
        Description? description,
        Color? color,
        HealthInfo? health,
        Address? address,
        Dimensions? dimensions,
        Phone? ownerPhoneNumber,
        bool? isCastrate,
        DateTime? dateOfBirth,
        bool? isVaccinated,
        int? helpStatus,
        IEnumerable<TransferDetails>? transferDetailsList
    )
    {
        updatedPet.Update(
            name,
            classification,
            description,
            color,
            health,
            address,
            dimensions,
            ownerPhoneNumber,
            isCastrate,
            dateOfBirth,
            isVaccinated,
            (HelpStatus?)helpStatus,
            transferDetailsList
            );
    }

    public void HardDeletePet(Pet pet)
    {
        _pets.Remove(pet);
    }

    public void SoftDeletePet(Pet pet)
    {
        pet.Delete();
    }

    public UnitResult<ErrorList> RestorePet(PetId petId)
    {
        var getPetResult = GetPetById(petId);
        if (getPetResult.IsFailure)
            return getPetResult.Error;
        
        getPetResult.Value.Restore();

        return Result.Success<ErrorList>();
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

    public void AddPetPhotos(PetId petId, IEnumerable<Photo> photos)
    {
        var pet = AllOwnedPets.FirstOrDefault(p => p.Id == petId)!;
        pet.AddPhotos(photos);
        
    }
    
    public UnitResult<ErrorList> DeletePetPhotos(PetId petId, IEnumerable<Photo> photos)
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
            var lastElement = petToMove.Max(p => p.Position.Value); foreach (var pet in petToMove)
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

    public Result<Pet, ErrorList> GetPetById(PetId petId)
    {
        var getPetResult = _pets.FirstOrDefault(p => p.Id == petId);
        if (getPetResult is null)
        {
            var errorMes = ($"Pet with id {petId.Value} not found for volunteer {Id.Value}");
            var error = Error.NotFound("value.not.found", errorMes);
            return new ErrorList([error]);
        }
        
        return getPetResult;
    }

    public UnitResult<ErrorList> ChangePetHelpStatus(PetId petId, HelpStatus helpStatus)
    {
        var getPetResult = GetPetById(petId);
        if (getPetResult.IsFailure)
            return getPetResult.Error;
        
        var pet = getPetResult.Value;
        
        pet.ChangeHelpStatus(helpStatus);
        
        return Result.Success<ErrorList>();
    }

    public UnitResult<ErrorList> SetPetMainPhoto(Pet pet, Photo photo)
    {
        var result = pet.SetMainPhoto(photo);
        if (result.IsFailure)
            return result.Error;
        return Result.Success<ErrorList>();
    }
    
    public UnitResult<ErrorList> RemovePetMainPhoto(Pet pet, Photo photo)
    {
        var result = pet.RemoveMainPhoto(photo);
        if (result.IsFailure)
            return result.Error;
        return Result.Success<ErrorList>();
    }

    public Result<Photo, ErrorList> GetPetPhoto(Pet pet, FilePath photoPath)
    {
        var result = pet.GetPhotoByPath(photoPath);
        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }

    private int CountPetsWithHome() => AllOwnedPets
        .Where(p => p.IsDeleted == false)
        .Count(p => p.HelpStatus == HelpStatus.FindHome);

    private int CountPetsTryFindHome() => AllOwnedPets
        .Where(p => p.IsDeleted == false)
        .Count(p => p.HelpStatus == HelpStatus.SeekHome); 
    
    private int CountPetsUnderTreatment() => AllOwnedPets
        .Where(p => p.IsDeleted == false)
        .Count(p => p.HelpStatus == HelpStatus.NeedHelp); 
}
