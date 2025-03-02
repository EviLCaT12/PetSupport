using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext.ValueObjects;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Domain.PetContext.Entities;

public class Pet : Entity<PetId>
{
    private bool _isDeleted = false;
    public PetId Id { get; private set; }
    
    public Volunteer Volunteer { get; private set; } = null!; //Для 100% каскадного удаления
    
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

    private List<PetPhoto> _photos = [];

    public IReadOnlyList<PetPhoto> PhotoList
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
        IEnumerable<PetPhoto> photoList)
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
        IEnumerable<PetPhoto> photoList)
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
    
    public void Delete() 
        => _isDeleted = true;
    
    public void Restore() 
        => _isDeleted = false;

    public void SetPosition(Position position) 
        => Position = position;

    public UnitResult<ErrorList> MoveForward(int minNumber ,int maxNumber)
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
    
    public UnitResult<ErrorList> MoveBackward(int minNumber, int maxNumber)
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
    
    public void AddPhotos(IEnumerable<PetPhoto> photos) 
        => _photos = photos.ToList();

    public UnitResult<ErrorList> DeletePhotos(IEnumerable<PetPhoto> photos)
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

    public void Update(
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
    
    public void ChangeHelpStatus(HelpStatus helpStatus)
        => HelpStatus = helpStatus;
}


public enum HelpStatus
{
    NeedHelp,
    SeekHome,
    FindHome
}