using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;

/// <summary>
/// Exception thrown when no primary key is found for an entity.
/// </summary>
public class NoPrimaryKeyException : GraphHandlerException
{
    internal NoPrimaryKeyException(EntityEntry entry) : base($"No primary key found for entity {entry.Entity.GetType()}.")
    {
    }
}