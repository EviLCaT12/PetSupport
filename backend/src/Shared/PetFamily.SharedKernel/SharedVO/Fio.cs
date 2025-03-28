using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Constants;
using PetFamily.SharedKernel.Error;

namespace PetFamily.SharedKernel.SharedVO;

public record Fio
{
    public string FirstName { get;}
    public string LastName { get;}
    public string Surname { get;}

    private Fio(string firstName, string lastName, string surname)
    {
        FirstName = firstName;
        LastName = lastName;
        Surname = surname;
    }

    public static Result<Fio, Error.Error> Create(string firstName, string lastName, string surname)
    {
        if (firstName.Length > VolunteerConstant.MAX_NAME_LENGTH ||
            lastName.Length > VolunteerConstant.MAX_NAME_LENGTH || surname.Length > VolunteerConstant.MAX_NAME_LENGTH)
            return Errors.General.LengthIsInvalid(VolunteerConstant.MAX_NAME_LENGTH);

        if (string.IsNullOrWhiteSpace(firstName))
            return Errors.General.ValueIsRequired(nameof(FirstName));
        
        if (string.IsNullOrWhiteSpace(lastName))
            return Errors.General.ValueIsRequired(nameof(LastName));
        
        if (string.IsNullOrWhiteSpace(surname))
            return Errors.General.ValueIsRequired(nameof(Surname));
        
        var validFio = new Fio(firstName, lastName, surname);
        
        return validFio;
    }
}