using PetFamily.Domain.PetContext.Entities;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    string firstName,
    string lastName,
    string surName,
    string phoneNumber, 
    string email,
    string description,
    int yearsOfExperience,
    string link,
    string socialWebName,
    List<Tuple<string, string>> transferDetails,
    List<Pet> allOwnedPets)
{
    
}