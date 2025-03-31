using PetFamily.Core.Dto.Shared;

namespace PetFamily.Core.Dto.AccountDto;

public class VolunteerAccountDto
{
    public Guid Id { get; init; }
    
    public Guid? UserId { get; init; }
    
    public Guid VolunteerId { get; init; }
    
    public int Experience { get; init; }
    
    public TransferDetailDto[] Requisites { get; init; } = [];
}