using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Constants;

namespace PetFamily.Domain.PetContext.ValueObjects.VolunteerVO;

public record VolunteerFio
{
    public string FirstName { get;}
    public string LastName { get;}
    public string Surname { get;}

    private VolunteerFio(string firstName, string lastName, string surname)
    {
        FirstName = firstName;
        LastName = lastName;
        Surname = surname;
    }

    public static Result<VolunteerFio> Create(string firstName, string lastName, string surname)
    {
        if (firstName.Length > VolunteerConstant.MAX_NAME_LENGTH || lastName.Length > VolunteerConstant.MAX_NAME_LENGTH || surname.Length > VolunteerConstant.MAX_NAME_LENGTH )
            return Result.Failure<VolunteerFio>("First name, second name and surname must be between 0 and 100 characters.");
                   
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<VolunteerFio>("First name cannot be null or empty.");
        
        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure<VolunteerFio>("Last Name name cannot be null or empty.");
        
        if (string.IsNullOrWhiteSpace(surname))
            return Result.Failure<VolunteerFio>("Surname name cannot be null or empty.");
        
        var fio = new VolunteerFio(firstName, lastName, surname);
        
        return Result.Success(fio);
    }
}