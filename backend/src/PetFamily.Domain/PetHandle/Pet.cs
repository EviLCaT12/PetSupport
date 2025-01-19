using CSharpFunctionalExtensions;
using PetFamily.Domain.PetInfo;

namespace PetFamily.Domain.PetHandle;

public class Pet
{
    public Guid Id { get; private set; }
    
    public string Name { get; private set; } = default!;
    
    public Guid SpeciesId { get; private set; }

    public string Description { get; private set; } = default!;
    
    public Guid BreedId { get; private set; }
    
    public string Color { get; private set; } = default!;
    
    public string HealthInfo { get; private set; } = default!;
    
    public string Address { get; private set; } = default!;
    
    public float Weight { get; private set; }
    
    public float Height { get; private set; }
    
    public string OwnerPhoneNumber { get; private set; } = default!;

    public bool IsCastrate { get; private set; } = false;
    
    public DateTime DateOfBirth { get; private set; } = default!;

    public bool IsVaccinated { get; private set; } = false;
    
    public HelpStatus HelpStatus { get; private set; } = HelpStatus.NeedHelp;
    
    public TransferDetails TransferDetails { get; private set; } = default!;

    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

    private Pet(
        string name,
        Guid speciesId,
        string description,
        Guid breedId,
        string color,
        string healthInfo,
        string address,
        float weight,
        float height,
        string ownerPhoneNumber,
        bool isCastrate,
        DateTime dateOfBirth,
        bool isVaccinated,
        TransferDetails transferDetails
    )
    {
        Name = name;
        SpeciesId = speciesId;
        Description = description;
        BreedId = breedId;
        Color = color;
        HealthInfo = healthInfo;
        Address = address;
        Weight = weight;
        Height = height;
        OwnerPhoneNumber = ownerPhoneNumber;
        IsCastrate = isCastrate;
        DateOfBirth = dateOfBirth;
        IsVaccinated = isVaccinated;
        TransferDetails = transferDetails;
    }

    public static Result<Pet> Create(
        string name,
        Guid speciesId,
        string description,
        Guid breedId,
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

        if (speciesId.Equals(Guid.Empty))
            return Result.Failure<Pet>("SpeciesId cannot be empty.");
        
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Pet>("Description cannot be null or empty.");
        
        if (breedId.Equals(Guid.Empty))
            return Result.Failure<Pet>("BreedId cannot be empty.");
        
        if (string.IsNullOrWhiteSpace(color))
            return Result.Failure<Pet>("Color cannot be null or empty.");
        
        if (string.IsNullOrWhiteSpace(address))
            return Result.Failure<Pet>("Address cannot be null or empty.");
        
        if (weight <= 0)
            return Result.Failure<Pet>("Weight cannot be less than 0.");
        
        if (height <= 0)
            return Result.Failure<Pet>("Height cannot be less than 0.");
        
        if (string.IsNullOrWhiteSpace(ownerPhoneNumber))
            return Result.Failure<Pet>("OwnerPhoneNumber cannot be null or empty.");

        var pet = new Pet(
            name,
            speciesId,
            description,
            breedId,
            color,
            healthInfo,
            address,
            weight,
            height,
            ownerPhoneNumber,
            isCastrate,
            dateOfBirth,
            isVaccinated,
            transferDetails);

        return Result.Success(pet);
    }
}



public enum HelpStatus
{
    NeedHelp,
    SeekHome,
    FindHome
}