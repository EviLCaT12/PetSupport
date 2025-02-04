namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerCommand(
    string FirstName,
    string LastName,
    string SurName,
    string PhoneNumber, 
    string Email,
    string Description,
    int YearsOfExperience);