using System.ComponentModel.DataAnnotations;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

public abstract class IdBase : IEquatable<IdBase>
{
    [Key] public int Id { get; set; }

    public bool Equals(IdBase? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        // Id == 0 is a new entity, which can not be returned from DB, so only the other values need to be compared.
        if (Id == 0) return true;
        return Id == other.Id;
    }
}