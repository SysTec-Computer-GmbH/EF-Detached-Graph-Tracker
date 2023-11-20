using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution;

public class ReferenceNavigationIdentityResolutionTests : TestBase<IdentityResolutionTestsDbContext>
{
    [Test]
    public async Task _01_TestIdentityResolution_ForReferenceNavigationAggregation_AndExistingComposition()
    {
        var rootNode = new RootNodeWithFirstTrackedCompositionCollection
        {
            A_Tracked_Items = new List<TrackedItem>
            {
                new()
                {
                    Text = "Item 1"
                },
                new()
                {
                    Text = "Item 2"
                },
                new()
                {
                    Text = "Item 3"
                }
            }
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var rootNodeUpdate = (RootNodeWithFirstTrackedCompositionCollection)rootNode.Clone();
        rootNodeUpdate.B_Tracked_Item = new TrackedItem
        {
            Id = rootNode.A_Tracked_Items[1].Id,
            Text = "Item 2 Update"
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootNodeUpdateFromDb = await dbContext.RootNodesWithFirstTrackedCompositionCollections
                .Include(r => r.A_Tracked_Items)
                .Include(r => r.B_Tracked_Item)
                .SingleAsync();

            rootNodeUpdateFromDb.A_Tracked_Items = rootNodeUpdate.A_Tracked_Items.OrderBy(i => i.Id).ToList();

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeUpdateFromDb.B_Tracked_Item.Text, Is.EqualTo("Item 2"));
                Assert.That(rootNodeUpdateFromDb.A_Tracked_Items[1].Text, Is.EqualTo("Item 2"));
                Assert.That(rootNodeUpdateFromDb.A_Tracked_Items[1].Id,
                    Is.EqualTo(rootNodeUpdateFromDb.B_Tracked_Item.Id));
            });
        }
    }

    [Test]
    public async Task _02_TestIdentityResolution_ForReferenceNavigationAggregation_AndExistingComposition()
    {
        var rootNode = new RootNodeWithFirstTrackedAggregationReference
        {
            B_Tracked_Item = new TrackedItem
            {
                Text = "Item 1"
            }
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var rootNodeUpdate = (RootNodeWithFirstTrackedAggregationReference)rootNode.Clone();
        rootNodeUpdate.A_Tracked_Item = new TrackedItem
        {
            Id = rootNode.B_Tracked_Item.Id,
            Text = "Item 1 Update"
        };
        rootNodeUpdate.B_Tracked_Item.Text = "Item 1 Correct Update";

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootNodeFromDb = await dbContext
                .RootNodesWithFirstTrackedAggregationReferences
                .Include(r => r.A_Tracked_Item)
                .Include(r => r.B_Tracked_Item)
                .SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.A_Tracked_Item.Text, Is.EqualTo("Item 1 Correct Update"));
                Assert.That(rootNodeFromDb.B_Tracked_Item.Text, Is.EqualTo("Item 1 Correct Update"));
                Assert.That(rootNodeFromDb.A_Tracked_Item.Id, Is.EqualTo(rootNodeFromDb.B_Tracked_Item.Id));
            });
        }
    }

    [Test]
    public async Task _03_TestIdentityResolution_ForReferenceNavigationComposition_AndExistingReferenceAggregation()
    {
        var subTreeComposition = DataHelper.GetSubTreeRootNode(DataHelper.COMPOSITION_NAME);

        var rootNode = new RootNodeWithFirstTrackedAggregationReferenceAndSubtree
        {
            B_Item = subTreeComposition
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var subTreeAggregation = (SubTreeRootNode)subTreeComposition.Clone();
        subTreeAggregation.Text = $"SubTree {DataHelper.AGGREGATION_NAME} Root";
        subTreeAggregation.ReferenceItem.Text = $"SubTree {DataHelper.AGGREGATION_NAME} Item";
        subTreeAggregation.ReferenceItem.SubTreeChildItems[0].Text =
            $"SubTree {DataHelper.AGGREGATION_NAME} Item Child 1";
        subTreeAggregation.ReferenceItem.SubTreeChildItems[1].Text =
            $"SubTree {DataHelper.AGGREGATION_NAME} Item Child 2";
        subTreeAggregation.SubTreeListItems[0].Text = $"SubTree List {DataHelper.AGGREGATION_NAME} Item 1";
        subTreeAggregation.SubTreeListItems[0].Text = $"SubTree List {DataHelper.AGGREGATION_NAME} Item 2";

        var rootNodeUpdate = (RootNodeWithFirstTrackedAggregationReferenceAndSubtree)rootNode.Clone();
        rootNodeUpdate.A_Item = subTreeAggregation;

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.RootNodeWithFirstTrackedAggregationReferencesAndSubtrees.SingleAsync();
            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.A_Item, Is.Not.Null);
                Assert.That(rootNodeFromDb.A_Item!.SubTreeListItems.Count, Is.EqualTo(2));
                AssertThatTreeValuesAreCompositionValues(rootNodeFromDb.A_Item!);

                Assert.That(rootNodeFromDb.B_Item, Is.Not.Null);
                Assert.That(rootNodeFromDb.B_Item.SubTreeListItems.Count, Is.EqualTo(2));
                AssertThatTreeValuesAreCompositionValues(rootNodeFromDb.B_Item);
            });
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var subTreeRootNodeCount = await dbContext.Set<SubTreeRootNode>().CountAsync();
            Assert.That(subTreeRootNodeCount, Is.EqualTo(1));

            var subTreeReferenceItemCount = await dbContext.Set<SubTreeReferenceItem>().CountAsync();
            Assert.That(subTreeReferenceItemCount, Is.EqualTo(1));

            var subTreeListItemCount = await dbContext.Set<SubTreeListItem>().CountAsync();
            Assert.That(subTreeListItemCount, Is.EqualTo(2));
        }
    }

    private void AssertThatTreeValuesAreCompositionValues(SubTreeRootNode aggregationTreeRootNode)
    {
        Assert.Multiple(() =>
        {
            Assert.That(aggregationTreeRootNode.Text, Is.EqualTo($"SubTree {DataHelper.COMPOSITION_NAME} Root"));
            Assert.That(aggregationTreeRootNode.ReferenceItem.Text,
                Is.EqualTo($"SubTree {DataHelper.COMPOSITION_NAME} Item"));
            Assert.That(aggregationTreeRootNode.ReferenceItem.SubTreeChildItems[0].Text,
                Is.EqualTo($"SubTree {DataHelper.COMPOSITION_NAME} Item Child 1"));
            Assert.That(aggregationTreeRootNode.ReferenceItem.SubTreeChildItems[1].Text,
                Is.EqualTo($"SubTree {DataHelper.COMPOSITION_NAME} Item Child 2"));
            Assert.That(aggregationTreeRootNode.SubTreeListItems[0].Text,
                Is.EqualTo($"SubTree List {DataHelper.COMPOSITION_NAME} Item 1"));
            Assert.That(aggregationTreeRootNode.SubTreeListItems[1].Text,
                Is.EqualTo($"SubTree List {DataHelper.COMPOSITION_NAME} Item 2"));
        });
    }
}