using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution;

public class ReferenceNavigationIdentityResolutionTests : TestBase<IdentityResolutionTestsDbContext>
{
    [Test]
    public async Task _01_TestIdentityResolution_ForReferenceNavigationAssociation_AndExistingComposition()
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
    public async Task _02_TestIdentityResolution_ForReferenceNavigationAssociation_AndExistingComposition()
    {
        var rootNode = new RootNodeWithFirstTrackedAssociationReference
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

        var rootNodeUpdate = (RootNodeWithFirstTrackedAssociationReference)rootNode.Clone();
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
                .RootNodesWithFirstTrackedAssociationReferences
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
    public async Task _03_TestIdentityResolution_ForReferenceNavigationComposition_AndExistingReferenceAssociation()
    {
        var subTreeComposition = DataHelper.GetSubTreeRootNode(DataHelper.COMPOSITION_NAME);

        var rootNode = new RootNodeWithFirstTrackedAssociationReferenceAndSubtree
        {
            B_Item = subTreeComposition
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var subTreeAssociation = (SubTreeRootNode)subTreeComposition.Clone();
        subTreeAssociation.Text = $"SubTree {DataHelper.ASSOCIATION_NAME} Root";
        subTreeAssociation.ReferenceItem.Text = $"SubTree {DataHelper.ASSOCIATION_NAME} Item";
        subTreeAssociation.ReferenceItem.SubTreeChildItems[0].Text =
            $"SubTree {DataHelper.ASSOCIATION_NAME} Item Child 1";
        subTreeAssociation.ReferenceItem.SubTreeChildItems[1].Text =
            $"SubTree {DataHelper.ASSOCIATION_NAME} Item Child 2";
        subTreeAssociation.SubTreeListItems[0].Text = $"SubTree List {DataHelper.ASSOCIATION_NAME} Item 1";
        subTreeAssociation.SubTreeListItems[0].Text = $"SubTree List {DataHelper.ASSOCIATION_NAME} Item 2";

        var rootNodeUpdate = (RootNodeWithFirstTrackedAssociationReferenceAndSubtree)rootNode.Clone();
        rootNodeUpdate.A_Item = subTreeAssociation;

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.RootNodeWithFirstTrackedAssociationReferencesAndSubtrees.SingleAsync();
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

    private void AssertThatTreeValuesAreCompositionValues(SubTreeRootNode associationTreeRootNode)
    {
        Assert.Multiple(() =>
        {
            Assert.That(associationTreeRootNode.Text, Is.EqualTo($"SubTree {DataHelper.COMPOSITION_NAME} Root"));
            Assert.That(associationTreeRootNode.ReferenceItem.Text,
                Is.EqualTo($"SubTree {DataHelper.COMPOSITION_NAME} Item"));
            Assert.That(associationTreeRootNode.ReferenceItem.SubTreeChildItems[0].Text,
                Is.EqualTo($"SubTree {DataHelper.COMPOSITION_NAME} Item Child 1"));
            Assert.That(associationTreeRootNode.ReferenceItem.SubTreeChildItems[1].Text,
                Is.EqualTo($"SubTree {DataHelper.COMPOSITION_NAME} Item Child 2"));
            Assert.That(associationTreeRootNode.SubTreeListItems[0].Text,
                Is.EqualTo($"SubTree List {DataHelper.COMPOSITION_NAME} Item 1"));
            Assert.That(associationTreeRootNode.SubTreeListItems[1].Text,
                Is.EqualTo($"SubTree List {DataHelper.COMPOSITION_NAME} Item 2"));
        });
    }
}