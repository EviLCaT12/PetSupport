using CSharpFunctionalExtensions;
using PetFamily.Domain.PetContext.ValueObjects;
using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Domain.PetContext.Entities;

public class Pet : Entity<PetId>
{
    public PetId Id { get; private set; }
    
    public Name Name { get; private set; }
    
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
        List<TransferDetails> transferDetails)
    {
        var nameCreateResult = Name.Create(name.Value);
            if(nameCreateResult.IsFailure)
                return Result.Failure<Pet, Error>(nameCreateResult.Error);

        var petClassificationCreateResult = PetClassification.Create(classification.SpeciesId, classification.BreedId);
        if (petClassificationCreateResult.IsFailure)
            return petClassificationCreateResult.Error;
        
        var descriptionCreateResult = Description.Create(description.Value);
        if(descriptionCreateResult.IsFailure)
            return descriptionCreateResult.Error;
        
        var healthInfoCreateResult = HealthInfo.Create(healthInfo.Value);
        if(healthInfoCreateResult.IsFailure)
            return healthInfoCreateResult.Error;

        var colorCreateResult = Color.Create(color.Value);
        if (colorCreateResult.IsFailure)
            return colorCreateResult.Error;
        
        var addressCreateResult = Address.Create(address.City, address.Street, address.HouseNumber);
        if (addressCreateResult.IsFailure)
            return addressCreateResult.Error; 

        var dimensionsResult = Dimensions.Create(dimensions.Height, dimensions.Weight);
        if (dimensionsResult.IsFailure)
            return dimensionsResult.Error;
        
        var phoneNumberCreateResult = Phone.Create(ownerPhoneNumber.Number);
        if (phoneNumberCreateResult.IsFailure)
            return phoneNumberCreateResult.Error;
        
        var transferDetailsListCreateResult = TransferDetailsList.Create(transferDetails);
        if(transferDetailsListCreateResult.IsFailure)
            return transferDetailsListCreateResult.Error;
        
        
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
            transferDetailsListCreateResult.Value);

        return pet;
    }
}



public enum HelpStatus
{
    NeedHelp,
    SeekHome,
    FindHome
}