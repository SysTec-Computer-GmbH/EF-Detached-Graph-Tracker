using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

internal static class EntityEntryStateHelper
{
    internal static void SetStateDependingOnKeyValue(EntityEntry entry)
    {
        if (entry.IsKeySet())
            entry.SetStateModified();
        else
            entry.SetStateAdded();
    }

    internal static void SetStateDetached(this EntityEntry entry)
    {
        entry.State = EntityState.Detached;
    }
    
    internal static void SetStateUnchanged(this EntityEntry entry)
    {
        entry.State = EntityState.Unchanged;
    }

    private static void SetStateModified(this EntityEntry entry)
    {
        entry.State = EntityState.Modified;
        SetGeneratedValuePropertiesToUnchanged(entry);
    }

    private static void SetStateAdded(this EntityEntry entry)
    {
        entry.State = EntityState.Added;
    }

    private static void SetGeneratedValuePropertiesToUnchanged(EntityEntry entry)
    {
        var generatedAlwaysProperties = entry.Properties.Where(p =>
                p.Metadata.GetValueGenerationStrategy() == NpgsqlValueGenerationStrategy.IdentityAlwaysColumn)
            .ToList();

        foreach (var property in generatedAlwaysProperties) property.IsModified = false;
    }
}