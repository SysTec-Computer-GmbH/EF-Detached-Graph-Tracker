using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models.GraphTraversal;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault;

public class EfDefaultGraphTraversalTests : TestBase<EfDefaultDbContext>
{
    [Test]
    [Category("Unexpected Behavior")]
    public void _01_GraphTraversal_PerformsAutomaticIdentityResolution_ForSameEntities_OnSameLayer_InReferenceNavigations()
    {
        var node = new TraversableNode();

        var root = new GraphTraversalRootNode()
        {
            A_Node = (TraversableNode)node.Clone(),
            B_Node = (TraversableNode)node.Clone()
        };
        
        Assert.That(ReferenceEquals(root.A_Node, root.B_Node), Is.False);

        using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.ChangeTracker.TrackGraph(root, TrackGraphCallback);
        }

        Assert.Multiple(() =>
        {
            // Why is automatic identity resolution happening here, even though not documented?
            Assert.That(ReferenceEquals(root.A_Node, root.B_Node), Is.True);

            Assert.That(root.GotTraversed, Is.True);
            Assert.That(root.A_Node.GotTraversed, Is.True);
            Assert.That(root.B_Node.GotTraversed, Is.True);
        });
    }
    
    [Test]
    public void _02_GraphTraversal_DoesNotPerformAutomaticIdentityResolution_ForSameEntities_OnSameLayer_InCollectionNavigations()
    {
        var node = new TraversableNode();

        var root = new GraphTraversalRootNode();
        root.A_Nodes.Add((TraversableNode) node.Clone());
        root.B_Nodes.Add((TraversableNode) node.Clone());

        Assert.That(ReferenceEquals(root.A_Nodes[0], root.B_Nodes[0]), Is.False);

        using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.ChangeTracker.TrackGraph(root, TrackGraphCallback);
        }

        Assert.Multiple(() =>
        {
            Assert.That(ReferenceEquals(root.A_Nodes[0], root.B_Nodes[0]), Is.False);

            Assert.That(root.GotTraversed, Is.True);
            Assert.That(root.A_Nodes[0].GotTraversed, Is.True);
            Assert.That(root.B_Nodes[0].GotTraversed, Is.True);
        });
    }
    
    [Test]
    [Category("Unexpected Behavior")]
    public void _03_GraphTraversal_PerformsAutomaticIdentityResolution_ForSameEntities_OnSameLayer_InReferenceNavigationAndCollectionNavigation()
    {
        var node = new TraversableNode();

        var root = new GraphTraversalRootNode
        {
            A_Node = (TraversableNode) node.Clone()
        };
        
        root.B_Nodes.Add((TraversableNode) node.Clone());

        Assert.That(ReferenceEquals(root.A_Node, root.B_Nodes[0]), Is.False);

        using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.ChangeTracker.TrackGraph(root, TrackGraphCallback);
        }

        Assert.Multiple(() =>
        {
            Assert.That(ReferenceEquals(root.A_Node, root.B_Nodes[0]), Is.False);

            Assert.That(root.GotTraversed, Is.True);
            Assert.That(root.A_Node.GotTraversed, Is.True);
            Assert.That(root.B_Nodes[0].GotTraversed, Is.True);
        });
    }
    
    [Test]
    [Category("Unexpected Behavior")]
    public void _04_GraphTraversal_PerformsAutomaticIdentityResolution_ForSameEntities_OnSameLayer_InCollectionNavigationAndReferenceNavigation()
    {
        var node = new TraversableNode();

        var root = new GraphTraversalRootNode
        {
            B_Node = (TraversableNode) node.Clone()
        };
        
        root.A_Nodes.Add((TraversableNode) node.Clone());

        Assert.That(ReferenceEquals(root.B_Node, root.A_Nodes[0]), Is.False);

        using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.ChangeTracker.TrackGraph(root, TrackGraphCallback);
        }

        Assert.Multiple(() =>
        {
            // Why is automatic identity resolution happening here, even though not documented?
            Assert.That(ReferenceEquals(root.B_Node, root.A_Nodes[0]), Is.True);

            Assert.That(root.GotTraversed, Is.True);
            Assert.That(root.B_Node.GotTraversed, Is.True);
            Assert.That(root.A_Nodes[0].GotTraversed, Is.True);
        });
    }
    
    [Test]
    public void _05_GraphTraversal_DoesNotPerformAutomaticIdentityResolution_ForSameEntities_OnDifferentLayers_InReferenceNavigations_WithItemOnRootFirstTracked()
    {
        var node = new TraversableNode();

        var root = new GraphTraversalRootNode()
        {
            A_Node = (TraversableNode)node.Clone(),
            B_SecondLayerNode = new()
            {
                A_Node = (TraversableNode)node.Clone()
            }
        };
        
        Assert.That(ReferenceEquals(root.A_Node, root.B_SecondLayerNode.A_Node), Is.False);

        using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.ChangeTracker.TrackGraph(root, TrackGraphCallback);
        }

        Assert.Multiple(() =>
        {
            Assert.That(ReferenceEquals(root.A_Node, root.B_SecondLayerNode.A_Node), Is.False);

            Assert.That(root.GotTraversed, Is.True);
            Assert.That(root.A_Node.GotTraversed, Is.True);
            Assert.That(root.B_SecondLayerNode.GotTraversed, Is.True);
            Assert.That(root.B_SecondLayerNode.A_Node.GotTraversed, Is.True);
        });
    }
    
    [Test]
    [Category("Unexpected Behavior")]
    public void _06_GraphTraversal_PerformsAutomaticIdentityResolution_ForSameEntities_OnDifferentLayers_InReferenceNavigations_WithItemOnSubLayerFirstTracked()
    {
        var node = new TraversableNode();

        var root = new GraphTraversalRootNode()
        {
            B_Node = (TraversableNode)node.Clone(),
            A_SecondLayerNode = new()
            {
                A_Node = (TraversableNode)node.Clone()
            }
        };
        
        Assert.That(ReferenceEquals(root.B_Node, root.A_SecondLayerNode.A_Node), Is.False);

        using (var dbContext = new EfDefaultDbContext())
        {
            dbContext.ChangeTracker.TrackGraph(root, TrackGraphCallback);
        }

        Assert.Multiple(() =>
        {
            // Why is automatic identity resolution happening here, even though not documented?
            Assert.That(ReferenceEquals(root.B_Node, root.A_SecondLayerNode.A_Node), Is.True);

            Assert.That(root.GotTraversed, Is.True);
            Assert.That(root.B_Node.GotTraversed, Is.True);
            Assert.That(root.A_SecondLayerNode.GotTraversed, Is.True);
            Assert.That(root.A_SecondLayerNode.A_Node.GotTraversed, Is.True);
        });
    }
    
    private void TrackGraphCallback(EntityEntryGraphNode node)
    {
        var iTraversableEntity = node.Entry.Entity as ITraversable;

        iTraversableEntity!.GotTraversed = true;

        if (ChangeTrackingHelper.FindEntryInChangeTracker(node.Entry) != null)
        {
            // This block is just used to prevent the entity is already tracked exception.
            // It has no effect on the graph traversal itself, so it does not affect the behavior.
            return;
        }
        
        node.Entry.State = EntityState.Modified;
    }
}