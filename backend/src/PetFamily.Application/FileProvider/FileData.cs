using PetFamily.Domain.PetContext.ValueObjects.PetVO;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Application.FileProvider;

public record FileData(Stream Stream, FilePath FilePath, string BucketName);