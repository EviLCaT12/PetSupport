using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Domain.Shared.SharedVO;

public record Phone
{
    public string Number { get; }

    private Phone(string number)
    {
        Number = number;
    }

    public static Result<Phone, Error.Error> Create(string number)
    {
        if(string.IsNullOrWhiteSpace(number))
            return Errors.General.ValueIsRequired(nameof(Number));
        
        const string phonePattern = @"^\+\d\s\(\d{3}\)\s\d{3}-\d{2}-\d{2}$";
        
        var isCorrect = Regex.IsMatch(number, phonePattern);
        
        if(!isCorrect)
            return Errors.General.ValueIsInvalid(nameof(Number));
        
        var validPhoneNumber = new Phone(number);
        
        return validPhoneNumber;
    }
    
}