using PetFamily.Application.Dto.Shared;

namespace PetFamily.Application.Dto.PetDto;

public class PetDto
{
    public bool IsDeleted { get; init; }
    
    public Guid Id { get; init; }
    
    public Guid VolunteerId { get; init; }
    
    public string  Name { get; init; } = string.Empty;

    public int Position { get; init; } 
    
    public Guid SpeciesId { get; init; }
    
    public Guid BreedId { get; init; }
    
    public string Description { get; init; } = string.Empty;
    
    public string Color { get; init; } = string.Empty;
    
    public string HealthInfo { get; init; } = string.Empty;
    
    public string City { get; init; } = string.Empty;
    
    public string Street { get; init; } = string.Empty;
    
    public string HouseNumber { get; init; } = string.Empty;
    
    public float Height { get; init; }
    
    public float Weight { get; init; }
    
    public string OwnerPhone { get; init; } = string.Empty;

    public bool IsCastrate { get; init; }
    
    public DateTime DateOfBirth { get; init; }

    public bool IsVaccinated { get; init; }
    
    public string HelpStatus { get;init; } = string.Empty;

    public string TransferDetails { get; init; } = string.Empty;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public PhotoDto[] Photos { get; init; }
}

public class PhotoDto
{
    public string PathToStorage { get; set; } = string.Empty;
}