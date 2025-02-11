namespace PetFamily.Application.FileProvider;

public record FileData(        
    Stream Stream,
    string Bucket,
    string ObjectName);