using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel;

public abstract class SoftDeleteableEntity<TId> : Entity<TId> where TId : IComparable<TId>
{
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    protected SoftDeleteableEntity(TId id) : base(id)
    {
    }

    public virtual void Delete()
    {
        if (IsDeleted) return;

        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    public virtual void Restore()
    {
        if (!IsDeleted) return;

        IsDeleted = false;
        DeletedAt = null;
    }
}
