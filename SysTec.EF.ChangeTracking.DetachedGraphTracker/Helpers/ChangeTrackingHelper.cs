using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

internal static class ChangeTrackingHelper
{
    internal static EntityEntry? FindEntryInChangeTracker(EntityEntry entry)
    {
        var entityType = entry.Metadata;
        var keys = entry.GetKeys();

        return entry.Context.ChangeTracker.Entries()
            .SingleOrDefault(
                e => Equals(e.Metadata, entityType)
                     && EqualityHelper.KeysAreEqual(e.GetKeys(), keys));
    }
}