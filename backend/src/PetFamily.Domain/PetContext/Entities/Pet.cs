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
    
    public TransferDetailsList TransferDetailsList { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

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
        TransferDetailsList transferDetails
    )
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
        TransferDetailsList = transferDetails;
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
        TransferDetailsList transferDetailsList)
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
            transferDetailsList);

        return pet;
    }
    
    public void Delete() 
        => _isDeleted = true;
    
    public void Restore() 
        => _isDeleted = false;

    public void SetPosition(Position position) 
        => Position = position;

    public UnitResult<Error> MoveForward()
    {
        var newPosition = Position.Forward();
        if (newPosition.IsFailure)
            return newPosition.Error;
        
        Position = newPosition.Value;

        return Result.Success<Error>();
    }
    
    public UnitResult<Error> MoveBackward()
    {
        var newPosition = Position.Backward();
        if (newPosition.IsFailure)
            return newPosition.Error;
        
        Position = newPosition.Value;

        return Result.Success<Error>();
    }
}


public enum HelpStatus
{
    NeedHelp,
    SeekHome,
    FindHome
}