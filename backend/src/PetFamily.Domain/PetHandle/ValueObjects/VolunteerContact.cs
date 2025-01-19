using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetHandle.ValueObjects;

public record VolunteerContact
{
    public string Email { get; }
    public string PhoneNumber { get; }

    private VolunteerContact(string email, string phoneNumber)
    {
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public static Result<VolunteerContact> Create(string email, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<VolunteerContact>("Email address cannot be null or empty.");
        
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return Result.Failure<VolunteerContact>("PhoneNumber cannot be null or empty.");
        
        var contacts = new VolunteerContact(email, phoneNumber);
        
        return Result.Success(contacts);
    }
    
}