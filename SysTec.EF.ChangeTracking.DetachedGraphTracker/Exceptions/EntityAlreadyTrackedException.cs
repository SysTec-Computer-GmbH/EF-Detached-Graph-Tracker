using Microsoft.EntityFrameworkCore.ChangeTracking;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;

/// <summary>
/// Exception thrown when an entity is already tracked before <see cref="DetachedGraphTracker.TrackGraphAsync"/> was called.
/// </summary>
public class EntityAlreadyTrackedException : GraphHandlerException
{
    internal EntityAlreadyTrackedException(EntityEntry existingEntryFromChangeTracker) : base(
        $"The entity {existingEntryFromChangeTracker.Metadata.Name} with the key {existingEntryFromChangeTracker.GetKeysDebugString()} is already tracked before {nameof(DetachedGraphTracker.TrackGraphAsync)} was called. Ensure that the entity is not tracked before calling TrackGraph.")
    {
    }
}