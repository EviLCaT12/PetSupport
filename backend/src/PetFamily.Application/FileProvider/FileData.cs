using PetFamily.Domain.PetContext.ValueObjects.PetVO;

namespace PetFamily.Application.FileProvider;

public record FileData(Stream Stream, FilePath FilePath, string BucketName);