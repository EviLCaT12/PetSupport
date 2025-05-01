using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Error;

namespace PetFamily.SharedKernel.SharedVO;

public class FileId : ValueObject
{
    private FileId() { }
    
    private FileId(Guid id) => Id = id;
    
    public Guid Id { get; private set; }

    public static Result<FileId, Error.Error> Create(Guid id)
    {
        if(id == Guid.Empty)
            return Errors.General.ValueIsRequired(nameof(Id));

        return new FileId(id);
    }
    public static FileId NewFileId() => new(Guid.NewGuid());

    public static FileId EmptyFileId() => new(Guid.Empty);
    
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Id;
    }
}