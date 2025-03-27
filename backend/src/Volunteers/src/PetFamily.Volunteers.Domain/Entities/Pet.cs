using CSharpFunctionalExtensions;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.Error;
using PetFamily.SharedKernel.SharedVO;
using PetFamily.Volunteers.Domain.ValueObjects.PetVO;

namespace PetFamily.Volunteers.Domain.Entities;

public class Pet : SoftDeletableEntity<PetId>
{
    public PetId Id { get; private set; }
    
    public Volunteer Volunteer { get; private set; } = null!;
    
    public Name Name { get; private set; }
    
    public Position Position { get; private set; }
    
    public PetClassification Classification { get; private set; }
    
    public Description Description { get; private set; }
    
    public Color Color { get; private set; }
    
    public HealthInfo HealthInfo { get; private set; }
    
    public Address Address { get; private set; }
    
    public Dimensions Dimensions { get; private set; }
    
    public Phone OwnerPhoneNumber { get; private set; }

    public bool IsCastrate { get; private set; }
    
    public DateTime DateOfBirth { get; private set; }

    public bool IsVaccinated { get; private set; }
    
    public HelpStatus HelpStatus { get; private set; } = HelpStatus.NeedHelp;

    private List<TransferDetails> _transferDetails = [];

    public IReadOnlyList<TransferDetails> TransferDetailsList
    {
        get => _transferDetails;
        private set => _transferDetails = value.ToList();
    }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private List<Photo> _photos = [];

    public IReadOnlyList<Photo> PhotoList
    {
        get => _photos;
        private set => _photos = value.ToList(); 
    }
    

    //ef core
    private Pet() {}
    
    private Pet(
        PetId id,
        Name name,
        PetClassification classification,
        Description description,
        Color color,
        HealthInfo healthInfo,
        Address address,
        Dimensions dimensions,
        Phone ownerPhoneNumber,
        bool isCastrate,
        DateTime dateOfBirth,
        bool isVaccinated,
        HelpStatus helpStatus,
        IEnumerable<TransferDetails> transferDetails,
        IEnumerable<Photo> photoList)
    {
        Id = id;
        Name = name;
        Classification = classification;
        Description = description;
        Color = color;
        HealthInfo = healthInfo;
        Address = address;
        Dimensions = dimensions;
        OwnerPhoneNumber = ownerPhoneNumber;
        IsCastrate = isCastrate;
        DateOfBirth = dateOfBirth;
        IsVaccinated = isVaccinated;
        HelpStatus = helpStatus;
        _transferDetails = transferDetails.ToList();
        _photos = photoList.ToList();
    }

    public static Result<Pet, Error> Create(
        PetId id,
        Name name,
        PetClassification classification,
        Description description,
        Color color,
        HealthInfo healthInfo,
        Address address,
        Dimensions dimensions,
        Phone ownerPhoneNumber,
        bool isCastrate,
        DateTime dateOfBirth,
        bool isVaccinated,
        string helpStatus,
        IEnumerable<TransferDetails> transferDetailsList,
        IEnumerable<Photo> photoList)
    {
        var pet = new Pet(
            id,
            name,
            classification,
            description,
            color, 
            healthInfo,
            address,
            dimensions,
            ownerPhoneNumber,
            isCastrate,
            dateOfBirth,
            isVaccinated,
            Enum.Parse<HelpStatus>(helpStatus),
            transferDetailsList,
            photoList);

        return pet;
    }

    internal void SetPosition(Position position) 
        => Position = position;

    internal UnitResult<ErrorList> MoveForward(int minNumber ,int maxNumber)
    {
        var newPosition = Position.Forward(minNumber ,maxNumber);
        if (newPosition.IsFailure)
        {
            var error = newPosition.Error;
            return new ErrorList([error]);
        }
        
        Position = newPosition.Value;

        return Result.Success<ErrorList>();
    }
    
    internal UnitResult<ErrorList> MoveBackward(int minNumber, int maxNumber)
    {
        var newPosition = Position.Backward(minNumber, maxNumber);
        if (newPosition.IsFailure)
        {
            var error = newPosition.Error;
            return new ErrorList([error]);
        }
        
        Position = newPosition.Value;

        return Result.Success<ErrorList>();
    }
    
    internal void AddPhotos(IEnumerable<Photo> photos) 
        => _photos = photos.ToList();

    internal UnitResult<ErrorList> DeletePhotos(IEnumerable<Photo> photos)
    {
        foreach (var photo in photos)
        {
            var removeResult = _photos.Remove(photo);
            if (removeResult == false)
            {
                var error = Error.NotFound("value.not.found", $"Photo {photo.PathToStorage.Path} was not found");
                return new ErrorList([error]);
            }
        }
        
        return Result.Success<ErrorList>();
    }

    internal UnitResult<ErrorList> SetMainPhoto(Photo photo)
    {
        if (Photo.CountMainPhoto > 0)
        {
            var error = Error.Failure("invalid.pet.operation", 
                $"Fail to set main to photo {photo.PathToStorage.Path}. Main Photo has already been set.");
            return new ErrorList([error]);
        }
    
        var newPetPhotoList = new List<Photo>();
        newPetPhotoList.AddRange(_photos);
        newPetPhotoList.Remove(photo);
        
        var newPhoto = Photo.Create(photo.PathToStorage).Value;
        var result = newPhoto.SetMain();
        if (result.IsFailure)
            return result.Error;
        
        newPetPhotoList.Insert(0, newPhoto);
        _photos = newPetPhotoList;
        
        Photo.CountMainPhoto += 1;
        
        return Result.Success<ErrorList>();
    }

    internal UnitResult<ErrorList> RemoveMainPhoto(Photo photo)
    {
        if (Photo.CountMainPhoto == 0)
        {
            var error = Error.Failure("invalid.pet.operation", 
                $"Fail to remove main from photo {photo.PathToStorage.Path}. Main Photo hasn`t already been set.");
            return new ErrorList([error]);
        }
    
        var newPetPhotoList = new List<Photo>();
        newPetPhotoList.AddRange(_photos);
        
        var result = photo.RemoveMain();
        if (result.IsFailure)
            return result.Error;
        
        _photos = newPetPhotoList;
        
        Photo.CountMainPhoto -= 1;
        
        return Result.Success<ErrorList>();
    }
    
    internal Result<Photo, ErrorList> GetPhotoByPath(FilePath path)
    {
        var photo = _photos.FirstOrDefault(p => p.PathToStorage == path);
        if (photo == null)
        {
            var error = Errors.General.ValueNotFound();
            return new ErrorList([error]);
        }
        
        return photo;
    }

    internal void Update(
        Name? name,
        PetClassification classification,
        Description? description,
        Color? color,
        HealthInfo? healthInfo,
        Address? address,
        Dimensions? dimensions,
        Phone? ownerPhoneNumber,
        bool? isCastrate,
        DateTime? dateOfBirth,
        bool? isVaccinated,
        HelpStatus? helpStatus,
        IEnumerable<TransferDetails>? transferDetails)
    {
        if (name != null) Name = name;
        Classification = classification; 
        if (description != null) Description = description;
        if (color != null) Color = color;
        if (healthInfo != null) HealthInfo = healthInfo;
        if (address != null) Address = address;
        if (dimensions != null) Dimensions = dimensions;
        if (ownerPhoneNumber != null) OwnerPhoneNumber = ownerPhoneNumber;
        IsCastrate = isCastrate ?? IsCastrate;
        DateOfBirth = dateOfBirth ?? DateOfBirth;
        IsVaccinated = isVaccinated ?? IsVaccinated;
        HelpStatus = helpStatus ?? HelpStatus;
        if (transferDetails != null) _transferDetails = transferDetails.ToList();
    }
    
    internal void ChangeHelpStatus(HelpStatus helpStatus)
        => HelpStatus = helpStatus;
}


public enum HelpStatus
{
    NeedHelp,
    SeekHome,
    FindHome
}