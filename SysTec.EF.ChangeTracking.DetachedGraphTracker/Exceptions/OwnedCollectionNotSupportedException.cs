using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;

/// <summary>
/// Exception thrown when an owned collection is found.<br/>
/// This is a known limitation of the library.
/// </summary>
public class OwnedCollectionNotSupportedException : GraphHandlerException
{
    internal OwnedCollectionNotSupportedException(CollectionEntry collectionEntry) : base(
        $"Owned collections are not supported. An owned collection was found in Type: {collectionEntry.EntityEntry.Entity.GetType()}. Property: {collectionEntry.Metadata.Name}.")
    {
    }
}