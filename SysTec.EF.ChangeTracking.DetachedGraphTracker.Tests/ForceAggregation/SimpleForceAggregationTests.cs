using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation;

public class SimpleForceAggregationTests : TestBase<ForceAggregationTestsDbContext>
{
    [Test]
    public async Task _01_ForceAggregationInReferenceNavigation_WithoutPreviousCompositionInChangeTracker()
    {
        var aggregationEntity = new AggregationType
        {
            Text = "AggregationType_1"
        };

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            dbContext.Add(aggregationEntity);
            await dbContext.SaveChangesAsync();
        }

        var rootNode = new ForceAggregationRootNode
        {
            Text = "Aggregation Root",
            AggregationTypeReference = new AggregationType
            {
                Id = aggregationEntity.Id,
                Text = "UpdatedAggregationType_1"
            }
        };

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNode);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var forceAggregationEntityFromDb = await dbContext
                .Set<AggregationType>()
                .SingleAsync(at => at.Id == aggregationEntity.Id);

            Assert.That(forceAggregationEntityFromDb.Text, Is.EqualTo(aggregationEntity.Text));
        }

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.ForceAggregationRootNodes
                .Include(x => x.AggregationTypeReference)
                .FirstOrDefaultAsync(x => x.Id == rootNode.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.Text, Is.EqualTo(rootNode.Text));
                Assert.That(rootNodeFromDb.AggregationTypeReference, Is.Not.Null);
                Assert.That(rootNodeFromDb.AggregationTypeReference.Text, Is.EqualTo(aggregationEntity.Text));
            });
        }
    }

    [Test]
    public async Task
        _02_ForceAggregationInCollectionNavigation_WithoutPreviousCompositionInChangeTracker()
    {
        var aggregationEntity1 = new AggregationType
        {
            Text = "AggregationType_1"
        };

        var aggregationEntity2 = new AggregationType
        {
            Text = "AggregationType_2"
        };

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            dbContext.AddRange(aggregationEntity1, aggregationEntity2);
            await dbContext.SaveChangesAsync();
        }

        var rootNode = new ForceAggregationRootNode
        {
            Text = "Aggregation Root",
            AggregationTypeCollection = new List<AggregationType>
            {
                new()
                {
                    Id = aggregationEntity1.Id,
                    Text = "UpdatedAggregationType_1"
                },
                new()
                {
                    Id = aggregationEntity2.Id,
                    Text = "UpdatedAggregationType_2"
                }
            }
        };

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); 
            await graphTracker.TrackGraphAsync(rootNode);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var aggregationEntity1FromDb = await dbContext
                .Set<AggregationType>()
                .SingleAsync(at => at.Id == aggregationEntity1.Id);

            Assert.That(aggregationEntity1FromDb.Text, Is.EqualTo(aggregationEntity1.Text));

            var aggregationEntity2FromDb = await dbContext
                .Set<AggregationType>()
                .SingleAsync(at => at.Id == aggregationEntity2.Id);

            Assert.That(aggregationEntity2FromDb.Text, Is.EqualTo(aggregationEntity2.Text));
        }

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.ForceAggregationRootNodes
                .Include(x => x.AggregationTypeCollection)
                .FirstOrDefaultAsync(x => x.Id == rootNode.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb!.AggregationTypeCollection, Has.Count.EqualTo(2));
                Assert.That(rootNodeFromDb.AggregationTypeCollection[0].Text, Is.EqualTo(aggregationEntity1.Text));
                Assert.That(rootNodeFromDb.AggregationTypeCollection[1].Text, Is.EqualTo(aggregationEntity2.Text));
            });
        }

        var rootNodeUpdate = new ForceAggregationRootNode()
        {
            Text = rootNode.Text,
            AggregationTypeCollection = new()
            {
                new()
                {
                    Id = rootNode.AggregationTypeCollection.First().Id
                }
            }
        };
        
        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); 
            await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.ForceAggregationRootNodes
                .Include(x => x.AggregationTypeCollection)
                .FirstOrDefaultAsync(x => x.Id == rootNode.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb!.AggregationTypeCollection, Has.Count.EqualTo(1)); 
            });
        }
    }
}