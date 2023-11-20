using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association;

public class SimpleAssociationTests : TestBase<AssociationTestsDbContext>
{
    [Test]
    public async Task _01_AssociationInReferenceNavigation_WithoutPreviousCompositionInChangeTracker()
    {
        var associationEntity = new AssociationType
        {
            Text = "AssociationType_1"
        };

        await using (var dbContext = new AssociationTestsDbContext())
        {
            dbContext.Add(associationEntity);
            await dbContext.SaveChangesAsync();
        }

        var rootNode = new AssociationRootNode
        {
            Text = "Association Root",
            AssociationTypeReference = new AssociationType
            {
                Id = associationEntity.Id,
                Text = "UpdatedAssociationType_1"
            }
        };

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNode);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var forceAssociationEntityFromDb = await dbContext
                .Set<AssociationType>()
                .SingleAsync(at => at.Id == associationEntity.Id);

            Assert.That(forceAssociationEntityFromDb.Text, Is.EqualTo(associationEntity.Text));
        }

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.AssociationRootNodes
                .Include(x => x.AssociationTypeReference)
                .FirstOrDefaultAsync(x => x.Id == rootNode.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.Text, Is.EqualTo(rootNode.Text));
                Assert.That(rootNodeFromDb.AssociationTypeReference, Is.Not.Null);
                Assert.That(rootNodeFromDb.AssociationTypeReference.Text, Is.EqualTo(associationEntity.Text));
            });
        }
    }

    [Test]
    public async Task
        _02_AssociationInCollectionNavigation_WithoutPreviousCompositionInChangeTracker()
    {
        var associationEntity1 = new AssociationType
        {
            Text = "AssociationType_1"
        };

        var associationEntity2 = new AssociationType
        {
            Text = "AssociationType_2"
        };

        await using (var dbContext = new AssociationTestsDbContext())
        {
            dbContext.AddRange(associationEntity1, associationEntity2);
            await dbContext.SaveChangesAsync();
        }

        var rootNode = new AssociationRootNode
        {
            Text = "Association Root",
            AssociationTypeCollection = new List<AssociationType>
            {
                new()
                {
                    Id = associationEntity1.Id,
                    Text = "UpdatedAssociationType_1"
                },
                new()
                {
                    Id = associationEntity2.Id,
                    Text = "UpdatedAssociationType_2"
                }
            }
        };

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); 
            await graphTracker.TrackGraphAsync(rootNode);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var associationEntity1FromDb = await dbContext
                .Set<AssociationType>()
                .SingleAsync(at => at.Id == associationEntity1.Id);

            Assert.That(associationEntity1FromDb.Text, Is.EqualTo(associationEntity1.Text));

            var associationEntity2FromDb = await dbContext
                .Set<AssociationType>()
                .SingleAsync(at => at.Id == associationEntity2.Id);

            Assert.That(associationEntity2FromDb.Text, Is.EqualTo(associationEntity2.Text));
        }

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.AssociationRootNodes
                .Include(x => x.AssociationTypeCollection)
                .FirstOrDefaultAsync(x => x.Id == rootNode.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb!.AssociationTypeCollection, Has.Count.EqualTo(2));
                Assert.That(rootNodeFromDb.AssociationTypeCollection[0].Text, Is.EqualTo(associationEntity1.Text));
                Assert.That(rootNodeFromDb.AssociationTypeCollection[1].Text, Is.EqualTo(associationEntity2.Text));
            });
        }

        var rootNodeUpdate = new AssociationRootNode()
        {
            Text = rootNode.Text,
            AssociationTypeCollection = new()
            {
                new()
                {
                    Id = rootNode.AssociationTypeCollection.First().Id
                }
            }
        };
        
        await using (var dbContext = new AssociationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); 
            await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new AssociationTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.AssociationRootNodes
                .Include(x => x.AssociationTypeCollection)
                .FirstOrDefaultAsync(x => x.Id == rootNode.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb!.AssociationTypeCollection, Has.Count.EqualTo(1)); 
            });
        }
    }
}