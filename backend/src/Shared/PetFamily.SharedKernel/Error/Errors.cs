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

        public static ErrorList ErrorDuringTransaction()
        {
            return Error.Failure("error.during.transaction", "Unexpected error during transaction").ToErrorList();
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

    public static class VolunteerRequest
    {
        public static ErrorList UserAlreadyVolunteer()
        {
            return Error.Validation("user.already.volunteer", "User is already volunteer").ToErrorList();
        }
        
        public static ErrorList RequestAlreadyOnReview()
        {
            return Error.Validation("request.already.onReview", "Request is already on review").ToErrorList();
        }
        
        public static ErrorList RequestAlreadyApproved()
        {
            return Error.Validation("request.already.approved", "Request is already approved").ToErrorList();
        }
        
        public static ErrorList RequestAlreadyRejected()
        {
            return Error.Validation("request.already.rejected", "Request is already rejected").ToErrorList();
        }        
        public static ErrorList RequestAlreadySendForRevision()
        {
            return Error.Validation("request.already.revision", "Request is already send to reve=ision")
                .ToErrorList();
        }
        public static ErrorList UserInTimeBan()
        {
            return Error.Conflict("user.is.banned", "User has been banned").ToErrorList();
        }

        public static ErrorList RequestIsNotOnRevision()
        {
            return Error.Validation("request.is.not.revision", "Request is not on revision").ToErrorList();
        }
    }
}