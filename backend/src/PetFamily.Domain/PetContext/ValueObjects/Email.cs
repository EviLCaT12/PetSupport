using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetContext.ValueObjects;

public record Email
{
    public string Value { get;}

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<Email>("Email cannot be empty");

        const string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        var isCorrect  = Regex.IsMatch(email, emailPattern);
        
        if(!isCorrect)
            return Result.Failure<Email>("Invalid email address");
        
        var correctEmail = new Email(email);
        
        return Result.Success(correctEmail);
        
    }
}