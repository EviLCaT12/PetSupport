using PetFamily.Domain.PetContext.ValueObjects.PetVO;

namespace PetFamily.Application.FileProvider;

public record FileRemoveData(FilePath FilePath, string BucketName);