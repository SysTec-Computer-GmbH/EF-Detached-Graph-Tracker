using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedPartialSubTreeTests;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution;

public class AdvancedPartialSubtreeIdentityResolutionTests : TestBase<IdentityResolutionTestsDbContext>
{
    [Test]
    public async Task
        _01_IdentityResolution_ForPartialTrackedSubtree_InReferenceNavigation_WithFirstTrackedComposition()
    {
        var root = new AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition
        {
            A_Composition = new AdvancedSubTreeNode2
            {
                Text = nameof(AdvancedSubTreeNode2)
            },
            B_Association = new AdvancedSubTreeNode1
            {
                Text = nameof(AdvancedSubTreeNode1)
            }
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }

        // The relationship between the entities can only be severed or connected when the advancedSubTreeNode1 is updated separately,
        // because in the tree it is an association which means nothing can be changed within its subtree.
        var association = (AdvancedSubTreeNode1)root.B_Association.Clone();
        association.CompositionNode = (AdvancedSubTreeNode2)root.A_Composition.Clone();

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(association);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate = (AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition)root.Clone();
        rootUpdate.B_Association = (AdvancedSubTreeNode1)association.Clone();
        rootUpdate.B_Association!.CompositionNode!.Text = "Updated";

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async () => await graphTracker.TrackGraphAsync(rootUpdate));
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition>()
                .SingleAsync(x => x.Id == root.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootFromDb.A_Composition!.Text, Is.EqualTo(nameof(AdvancedSubTreeNode2)));
                Assert.That(rootFromDb.B_Association!.CompositionNode!.Text, Is.EqualTo(nameof(AdvancedSubTreeNode2)));
            });
        }

        var rootUpdate2 = (AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition)rootUpdate.Clone();
        rootUpdate2.B_Association!.CompositionNode = null;

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootUpdate2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition>()
                .SingleAsync(x => x.Id == root.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootFromDb.A_Composition, Is.Not.Null);
                Assert.That(rootFromDb.B_Association!.CompositionNode, Is.Not.Null);
            });
        }
    }

    [Test]
    public async Task
        _02_IdentityResolution_ForPartialTrackedSubtree_InReferenceNavigation_WithFirstTrackedAssociation()
    {
        var root = new AdvancedPartialSubTreeRootNodeWithFirstTrackedAssociation
        {
            A_Association = new AdvancedSubTreeNode1
            {
                Text = nameof(AdvancedSubTreeNode1)
            },
            B_Composition = new AdvancedSubTreeNode2
            {
                Text = nameof(AdvancedSubTreeNode2)
            }
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }

        // The relationship between the entities can only be severed or connected when the advancedSubTreeNode1 is updated separately,
        // because in the tree it is an association which means nothing can be changed within its subtree.
        var association = (AdvancedSubTreeNode1)root.A_Association.Clone();
        association.CompositionNode = (AdvancedSubTreeNode2)root.B_Composition.Clone();

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(association);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate = (AdvancedPartialSubTreeRootNodeWithFirstTrackedAssociation)root.Clone();
        rootUpdate.A_Association = (AdvancedSubTreeNode1)association.Clone();
        rootUpdate.B_Composition.Text = "Updated";

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async () =>  await graphTracker.TrackGraphAsync(rootUpdate));
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<AdvancedPartialSubTreeRootNodeWithFirstTrackedAssociation>()
                .SingleAsync(x => x.Id == root.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootFromDb.A_Association!.CompositionNode!.Text, Is.EqualTo("Updated"));
                Assert.That(rootFromDb.B_Composition.Text, Is.EqualTo("Updated"));
            });
        }

        var rootUpdate2 = (AdvancedPartialSubTreeRootNodeWithFirstTrackedAssociation)rootUpdate.Clone();
        rootUpdate2.A_Association!.CompositionNode = null;

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async () =>  await graphTracker.TrackGraphAsync(rootUpdate2));
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<AdvancedPartialSubTreeRootNodeWithFirstTrackedAssociation>()
                .SingleAsync(x => x.Id == root.Id);

            Assert.Multiple(() =>
            {
                // In an association subtree nothing is allowed to be changed.
                Assert.That(rootFromDb.A_Association!.CompositionNode, Is.Not.Null);
                Assert.That(rootFromDb.B_Composition, Is.Not.Null);
            });
        }
    }

    [Test]
    public async Task
        _03_IdentityResolution_ForPartialTrackedSubtree_InCollectionNavigation_WithFirstTrackedComposition()
    {
        var root = new AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition
        {
            A_Compositions = new List<AdvancedSubTreeNode2>
            {
                new()
                {
                    Text = nameof(AdvancedSubTreeNode2)
                }
            },
            B_Associations = new List<AdvancedSubTreeNode1>
            {
                new AdvancedSubTreeNode1
                {
                    Text = nameof(AdvancedSubTreeNode1)
                }
            }
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }

        // The relationship between the entities can only be severed or connected when the advancedSubTreeNode1 is updated separately,
        // because in the tree it is an association which means nothing can be changed within its subtree.
        var association = (AdvancedSubTreeNode1)root.B_Associations[0].Clone();
        association.CompositionNode = (AdvancedSubTreeNode2)root.A_Compositions[0].Clone();

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(association);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate = (AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition)root.Clone();
        rootUpdate.B_Associations.Clear();
        rootUpdate.B_Associations.Add((AdvancedSubTreeNode1)association.Clone());
        rootUpdate.B_Associations[0].CompositionNode!.Text = "Updated";

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async () => await graphTracker.TrackGraphAsync(rootUpdate));
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition>()
                .SingleAsync(x => x.Id == root.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootFromDb.A_Compositions[0].Text, Is.EqualTo(nameof(AdvancedSubTreeNode2)));
                Assert.That(rootFromDb.B_Associations[0].CompositionNode!.Text,
                    Is.EqualTo(nameof(AdvancedSubTreeNode2)));
            });
        }

        var rootUpdate2 = (AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition)rootUpdate.Clone();
        rootUpdate2.B_Associations[0].CompositionNode = null;

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootUpdate2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition>()
                .SingleAsync(x => x.Id == root.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootFromDb.A_Compositions, Has.Count.EqualTo(1));
                Assert.That(rootFromDb.B_Associations[0].CompositionNode, Is.Not.Null);
            });
        }
    }

    [Test]
    public async Task
        _04_IdentityResolution_ForPartialTrackedSubtree_InCollectionNavigation_WithFirstTrackedAssociation()
    {
        var root = new AdvancedPartialSubTreeRootNodeWithFirstTrackedAssociation
        {
            A_Associations = new List<AdvancedSubTreeNode1>
            {
                new()
                {
                    Text = nameof(AdvancedSubTreeNode1)
                }
            },
            B_Compositions = new List<AdvancedSubTreeNode2>
            {
                new AdvancedSubTreeNode2
                {
                    Text = nameof(AdvancedSubTreeNode2)
                }
            }
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }

        // The relationship between the entities can only be severed or connected when the advancedSubTreeNode1 is updated separately,
        // because in the tree it is an association which means nothing can be changed within its subtree.
        var association = (AdvancedSubTreeNode1)root.A_Associations[0].Clone();
        association.CompositionNode = (AdvancedSubTreeNode2)root.B_Compositions[0].Clone();

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(association);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate = (AdvancedPartialSubTreeRootNodeWithFirstTrackedAssociation)root.Clone();
        rootUpdate.A_Associations.Clear();
        rootUpdate.A_Associations.Add((AdvancedSubTreeNode1)association.Clone());
        rootUpdate.B_Compositions[0].Text = "Updated";

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async () =>  await graphTracker.TrackGraphAsync(rootUpdate));
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<AdvancedPartialSubTreeRootNodeWithFirstTrackedAssociation>()
                .SingleAsync(x => x.Id == root.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootFromDb.A_Associations[0].CompositionNode!.Text, Is.EqualTo("Updated"));
                Assert.That(rootFromDb.B_Compositions[0].Text, Is.EqualTo("Updated"));
            });
        }

        var rootUpdate2 = (AdvancedPartialSubTreeRootNodeWithFirstTrackedAssociation)rootUpdate.Clone();
        rootUpdate2.A_Associations[0]!.CompositionNode = null;

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async () =>  await graphTracker.TrackGraphAsync(rootUpdate2));
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<AdvancedPartialSubTreeRootNodeWithFirstTrackedAssociation>()
                .SingleAsync(x => x.Id == root.Id);

            Assert.Multiple(() =>
            {
                // In an association subtree nothing is allowed to be changed.
                Assert.That(rootFromDb.A_Associations[0].CompositionNode, Is.Not.Null);
                Assert.That(rootFromDb.B_Compositions, Has.Count.EqualTo(1));
            });
        }
    }
}