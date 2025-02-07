namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public record UpdateVolunteerCommand(
    Guid VolunteerId,
    string FirstName,
    string LastName,
    string SurName,
    string Phone,
    string Email,
    string Description,
    int YearsOfExperience);