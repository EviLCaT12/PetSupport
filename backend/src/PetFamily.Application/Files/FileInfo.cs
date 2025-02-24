using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.Files;

public record FileInfo(FilePath FilePath, string BucketName);