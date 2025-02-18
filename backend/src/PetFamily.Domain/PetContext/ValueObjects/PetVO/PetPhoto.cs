using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;
using PetFamily.Domain.Shared.SharedVO;

namespace PetFamily.Domain.PetContext.ValueObjects.PetVO;

public record PetPhoto
{
    //ef core
    public PetPhoto() { }
    private PetPhoto(FilePath pathToStorage)
    {
        PathToStorage = pathToStorage;
    }
    public FilePath PathToStorage { get; }

    public static Result<PetPhoto, ErrorList> Create(FilePath photoFilePath)
    {
        return new PetPhoto(photoFilePath);
    }
}
