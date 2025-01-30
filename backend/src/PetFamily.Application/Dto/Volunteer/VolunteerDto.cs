namespace PetFamily.Application.Dto.Volunteer;

public record VolunteerDto(
    string FirstName,
    string LastName,
    string SurName,
    string PhoneNumber, 
    string Email,
    string Description,
    int YearsOfExperience);