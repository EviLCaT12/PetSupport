namespace PetFamily.SharedKernel.Error;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsRequired(string? propertyName = null)
        {
            var text = propertyName ?? "value";
            return Error.Validation("value.is.required", $"{text} is required");
        }
        
        public static Error Failure(string? propertyName = null)
        {
            return Error.Failure("failure", "Failure");
        }

        public static Error ValueIsInvalid(string? propertyName = null)
        {
            var text = propertyName ?? "value";
            return Error.Validation("value.is.invalid", $"{text} is invalid");
        }

        public static Error ValueNotFound(Guid? id = null)
        {
            var text = id == null ? "" : $" for id {id}";
            return Error.Validation("record.not.found", $"record not found{text}");
        }

        public static Error LengthIsInvalid(int lessThen, string? propertyName = null)
        {
            var text = propertyName ?? ""; 
            return Error.Validation("value.length.invalid", $"{text} length is invalid. Maximum length is {lessThen}.");
        }

        public static Error AlreadyExist(string? propertyName = null)
        {
            var text = String.IsNullOrEmpty(propertyName)
                ? "Record already exists"
                : $"Record {propertyName} already exists";
            return Error.Validation("record.already.exists", $"{text}");
        }
    }
    
    public static class Tokens
    {
        public static ErrorList ExpiredToken()
        {
            return Error.Validation("token.is.expired", "Token is expired").ToErrorList();
        }
        public static ErrorList InvalidToken()
        {
            return Error.Validation("token.is.invalid", "Token is invalid").ToErrorList();
        }
    }

    public static class User
    {
        public static Error InvalidCredentials()
        {
            return Error.Validation("credentials.is.invalid", "Invalid credentials");
        }
    }
}