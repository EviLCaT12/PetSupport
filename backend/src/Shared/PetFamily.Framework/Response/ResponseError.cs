namespace PetFamily.Framework.Response;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);