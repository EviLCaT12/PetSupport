using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext.ValueObjects;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.SharedVO;

namespace PetFamily.Domain.PetContext.Entities;

public class Pet : Entity<PetId>
{
    public PetId Id { get; private set; }
    
    public Name Name { get; private set; }
    
    public PetClassification Classification { get; private set; }
    
    public Description Description { get; private set; }
    
    public Color Color { get; private set; }
    
    public Description HealthInfo { get; private set; }
    
    public Address Address { get; private set; }
    
    public Dimensions Dimensions { get; private set; }
    
    public Phone OwnerPhoneNumber { get; private set; }

    public bool IsCastrate { get; private set; } = false;
    
    public DateTime DateOfBirth { get; private set; } = default!;

    public bool IsVaccinated { get; private set; } = false;
    
    public HelpStatus HelpStatus { get; private set; } = HelpStatus.NeedHelp;
    
    public TransferDetails TransferDetails { get; private set; }

    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

    private Pet(
        PetId id,
        Name name,
        PetClassification classification,
        Description description,
        Color color,
        Description healthInfo,
        Address address,
        Dimensions dimensions,
        Phone ownerPhoneNumber,
        bool isCastrate,
        DateTime dateOfBirth,
        bool isVaccinated,
        TransferDetails transferDetails
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
        TransferDetails = transferDetails;
    }

    public static Result<Pet> Create(
        PetId id,
        Name name,
        PetClassification classification,
        Description description,
        Color color,
        Description healthInfo,
        Address address,
        Dimensions dimensions,
        Phone ownerPhoneNumber,
        bool isCastrate,
        DateTime dateOfBirth,
        bool isVaccinated,
        TransferDetails transferDetails)
    {
        var nameCreateResult = Name.Create(name.Value);
            if(nameCreateResult.IsFailure)
                return Result.Failure<Pet>(nameCreateResult.Error);

        var petClassificationCreateResult = PetClassification.Create(classification.SpeciesId, classification.BreedId);
        if (petClassificationCreateResult.IsFailure)
            return Result.Failure<Pet>(petClassificationCreateResult.Error);
        
        var descriptionCreateResult = Description.Create(description.Value);
        if(descriptionCreateResult.IsFailure)
            return Result.Failure<Pet>(descriptionCreateResult.Error);
        
        var healthInfoCreateResult = Description.Create(healthInfo.Value);
        if(healthInfoCreateResult.IsFailure)
            return Result.Failure<Pet>(healthInfoCreateResult.Error);

        var colorCreateResult = Color.Create(color.Value);
        if (colorCreateResult.IsFailure)
            return Result.Failure<Pet>(colorCreateResult.Error);
        
        var addressCreateResult = Address.Create(address.City, address.Street, address.HouseNumber);
        if (addressCreateResult.IsFailure)
            return Result.Failure<Pet>(addressCreateResult.Error);
        
        var dimensionsResult = Dimensions.Create(dimensions.Height, dimensions.Weight);
        if (dimensionsResult.IsFailure)
            return Result.Failure<Pet>(dimensionsResult.Error);
        
        var phoneNumberCreateResult = Phone.Create(ownerPhoneNumber.Number);
        if (phoneNumberCreateResult.IsFailure)
            return Result.Failure<Pet>(phoneNumberCreateResult.Error);
        
        var transferDetailsCreateResult = TransferDetails.Create(transferDetails.Name, transferDetails.Description );
        if (transferDetailsCreateResult.IsFailure)
            return Result.Failure<Pet>(transferDetailsCreateResult.Error);
        
        
        var pet = new Pet(
            id,
            nameCreateResult.Value,
            petClassificationCreateResult.Value,
            descriptionCreateResult.Value,
            colorCreateResult.Value, 
            healthInfoCreateResult.Value,
            addressCreateResult.Value,
            dimensionsResult.Value,
            phoneNumberCreateResult.Value,
            isCastrate,
            dateOfBirth,
            isVaccinated,
            transferDetailsCreateResult.Value);

        return Result.Success(pet);
    }
}



public enum HelpStatus
{
    NeedHelp,
    SeekHome,
    FindHome
}