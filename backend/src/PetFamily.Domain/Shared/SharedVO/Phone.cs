using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared.SharedVO;

public record Phone
{
    public string Number { get; }

    private Phone(string number)
    {
        Number = number;
    }

    public static Result<Phone> Create(string number)
    {
        if(string.IsNullOrWhiteSpace(number))
            return Result.Failure<Phone>("Phone number cannot be empty");
        
        const string phonePattern = @"^\+\d\s\(\d{3}\)\s\d{3}-\d{2}-\d{2}$";
        
        var isCorrect = Regex.IsMatch(number, phonePattern);
        
        if(!isCorrect)
            return Result.Failure<Phone>("Phone number is not valid");
        
        var phoneNumber = new Phone(number);
        
        return Result.Success(phoneNumber);
    }
    
}