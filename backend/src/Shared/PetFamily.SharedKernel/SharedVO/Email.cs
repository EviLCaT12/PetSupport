using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;

namespace PetFamily.SharedKernel.SharedVO;

public record Email
{
    public string Value { get;}

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email, Error.Error> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Errors.General.ValueIsRequired(nameof(Email));

        const string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        var isCorrect  = Regex.IsMatch(email, emailPattern);
        
        if(!isCorrect)
            return Errors.General.ValueIsInvalid(nameof(Email));
        
        var validEmail = new Email(email);

        return validEmail;

    }
}