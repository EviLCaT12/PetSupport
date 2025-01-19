using CSharpFunctionalExtensions;
using PetFamily.Domain.PetHandle.ValueObjects;

namespace PetFamily.Domain.PetHandle.Entities;

public class Pet : Entity
{
    public PetId Id { get; private set; }
    
    public string Name { get; private set; }
    
    public PetClassification Classification { get; private set; }
    
    public string Description { get; private set; }
    
    
    public Color Color { get; private set; }
    
    public string HealthInfo { get; private set; }
    
    public string Address { get; private set; }
    
    public Dimensions Dimensions { get; private set; }
    
    public OwnerPhoneNumber OwnerPhoneNumber { get; private set; }

    public bool IsCastrate { get; private set; } = false;
    
    public DateTime DateOfBirth { get; private set; } = default!;

    public bool IsVaccinated { get; private set; } = false;
    
    public HelpStatus HelpStatus { get; private set; } = HelpStatus.NeedHelp;
    
    public TransferDetails TransferDetails { get; private set; }

    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

    private Pet(
        string name,
        PetClassification classification,
        string description,
        Color color,
        string healthInfo,
        string address,
        Dimensions dimensions,
        OwnerPhoneNumber ownerPhoneNumber,
        bool isCastrate,
        DateTime dateOfBirth,
        bool isVaccinated,
        TransferDetails transferDetails
    )
    {
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
        string name,
        PetClassification classification,
        string description,
        string color,
        string healthInfo,
        string address,
        float weight,
        float height,
        string ownerPhoneNumber,
        bool isCastrate,
        DateTime dateOfBirth,
        bool isVaccinated,
        TransferDetails transferDetails)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Pet>("Name cannot be null or empty.");

        var petClassificationCreateResult = PetClassification.Create(classification.SpeciesId, classification.BreedId);
        if (petClassificationCreateResult.IsFailure)
            return Result.Failure<Pet>(petClassificationCreateResult.Error);
        
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Pet>("Description cannot be null or empty.");
        
        
        var colorCreateResult = Color.Create(color);
        if (colorCreateResult.IsFailure)
            return Result.Failure<Pet>(colorCreateResult.Error);
        
        if (string.IsNullOrWhiteSpace(address))
            return Result.Failure<Pet>("Address cannot be null or empty.");
        
        var dimensionsResult = Dimensions.Create(weight, height);
        if (dimensionsResult.IsFailure)
            return Result.Failure<Pet>(dimensionsResult.Error);
        
        var phoneNumberCreateResult = OwnerPhoneNumber.Create(ownerPhoneNumber);
        if (phoneNumberCreateResult.IsFailure)
            return Result.Failure<Pet>(phoneNumberCreateResult.Error);
        
        var transferDetailsCreateResult = TransferDetails.Create(transferDetails.IdValue, transferDetails.NameValue, transferDetails.DescriptionValue );
        if (transferDetailsCreateResult.IsFailure)
            return Result.Failure<Pet>(transferDetailsCreateResult.Error);
        
        
        var pet = new Pet(
            name,
            petClassificationCreateResult.Value,
            description,
            colorCreateResult.Value, 
            healthInfo,
            address,
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