using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.ChangeTracking;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

[assembly: InternalsVisibleTo(assemblyName:"SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests")] 
namespace SysTec.EF.ChangeTracking.DetachedGraphTracker;

/// <summary>
/// <para>
/// This class is responsible for tracking the graph of entities using the <see cref="ChangeTracker.TrackGraph"/> method.
/// It handles missing items in collection navigations, identity resolution and change tracking.
/// </para>
/// <para>
/// <list type="bullet">
/// <listheader><term><b>Usage:</b></term></listheader>
/// <item><term>
/// Make sure a single instance is used per request in a detached scenario.
/// <br/> For Web-API: Register the <see cref="DetachedGraphTracker"/> as a scoped service in the <see cref="IServiceCollection"/>
/// </term></item>
/// <item><term> For tracking a graph call <see cref="TrackGraphAsync"/></term></item>
/// </list>
/// </para>
/// </summary>
public class DetachedGraphTracker
{
    private readonly ChangeTrackingHandler _changeTrackingHandler;
    private readonly CollectionNavigationUpdateHandler _collectionNavigationUpdateHandler;
    private readonly IdentityResolutionHandler _identityResolutionHandler;
    private readonly DbContext _dbContext;

    /// <summary>
    /// Creates a new instance of <see cref="DetachedGraphTracker"/>
    /// </summary>
    /// <param name="dbContext">The <see cref="DbContext"/> which should be used to track the changes.</param>
    public DetachedGraphTracker(DbContext dbContext)
    {
        _changeTrackingHandler = new ChangeTrackingHandler();
        _identityResolutionHandler = new IdentityResolutionHandler(_changeTrackingHandler);
        _collectionNavigationUpdateHandler = new CollectionNavigationUpdateHandler();
        _dbContext = dbContext;
    }

    /// <summary>
    /// <list type="bullet">
    /// <listheader><term><b>Tracks a graph of entities:</b></term></listheader>
    /// <item><term>
    /// Sets the state of every entity in the graph in respect to its key value and attributes.
    /// <seealso cref="ForceAggregationAttribute"/> <seealso cref="ForceDeleteOnMissingEntriesAttribute"/><seealso cref="ForceKeepExistingRelationship"/>
    /// </term></item>
    /// <item><term>Takes care of identity resolution automatically.</term></item>
    /// <item><term>
    /// Handles missing items in collection navigations.<br/>
    /// Normally EF does not sever relationships if a collection navigation is null or empty or the collection differs from the actual persisted database values.
    /// This method will sever the relationships in these cases. Cascading actions may occur depending on the <see cref="DeleteBehavior"/> of the relationship.
    /// </term></item>
    /// </list>
    /// </summary>
    /// <param name="rootEntity">The node to begin tracking with.</param>
    public async Task TrackGraphAsync(object rootEntity)
    {
        try
        {
            _dbContext.ChangeTracker.TrackGraph(rootEntity, TrackGraphNode);
            _identityResolutionHandler.PerformIdentityResolutionForUntrackedAggregations();
            await _collectionNavigationUpdateHandler.TrackListUpdates();
        }
        finally
        {
            Cleanup();
        }
    }

    private void TrackGraphNode(EntityEntryGraphNode node)
    {
        var hasExistingCompositionInChangeTracker = _changeTrackingHandler.HasExistingCompositionInChangeTracker(node.Entry);

        if (!hasExistingCompositionInChangeTracker && !node.IsAggregation())
        {
            EntityEntryStateHelper.SetStateDependingOnKeyValue(node.Entry);
            NavigationEntryHelper.KeepUnchangedStateForForceUnchangedNavigations(node);
            NavigationEntryHelper.SeverRelationshipsForNullValuesInReferenceNavigations(node);
        }
        else if (!node.InboundNavigationHasForceAggregationAttribute())
        {
            ThrowHelper.ThrowMultipleCompositionException(node.Entry.Entity.GetType());
        }

        _changeTrackingHandler.AddTrackedEntity(node);
        _collectionNavigationUpdateHandler.PrepareListUpdateHandling(node);
    }

    private void Cleanup()
    {
        _collectionNavigationUpdateHandler.Cleanup();
        _changeTrackingHandler.Cleanup();
    }
}