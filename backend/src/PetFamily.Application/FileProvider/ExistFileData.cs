using PetFamily.Domain.PetContext.ValueObjects.PetVO;

namespace PetFamily.Application.FileProvider;

public record ExistFileData(FilePath FilePath, string BucketName);