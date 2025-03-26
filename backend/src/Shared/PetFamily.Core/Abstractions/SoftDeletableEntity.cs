using CSharpFunctionalExtensions;

namespace PetFamily.Core.Abstractions;

public abstract class SoftDeletableEntity<TId> : Entity<TId> where TId : IComparable<TId>
{
    public bool IsDeleted { get; private set; }
    
    public DateTime? DeletedOn { get; private set; }


    public virtual void Delete()
    {
        IsDeleted = true;
        DeletedOn = DateTime.UtcNow;
    }

    public virtual void Restore()
    {
        IsDeleted = false;
        DeletedOn = null;
    }
}