using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.ManyToMany;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships;

public class ManyToManyWithSkipNavigationTests : TestBase<RelationshipTestsDbContext>
{
    [Test]
    public async Task _01_ManyToManyRelationships_WithSkipNavigation_CanBeConnectedAndSevered()
    {
        var manyEntityOne = new ManyEntityOne
        {
            Text = "ManyEntityOne",
            ManyCompositions = new List<ManyEntityTwo>
            {
                new()
                {
                    Text = "ManyEntityTwo"
                }
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyEntityOne);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var manyEntityOneFromDb = await dbContext.ManyEntityOnes
                .Include(x => x.ManyCompositions)
                .SingleAsync(x => x.Id == manyEntityOne.Id);

            Assert.That(manyEntityOneFromDb.ManyCompositions, Has.Count.EqualTo(1));
        }

        var manyEntityTwo = manyEntityOne.ManyCompositions.First();
        var manyEntityTwoUpdate = new ManyEntityTwo
        {
            Id = manyEntityTwo.Id,
            Text = "Update",
            EntityOneAggregations = new List<ManyEntityOne>()
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyEntityTwoUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var manyEntityOneFromDb = await dbContext.ManyEntityOnes
                .Include(x => x.ManyCompositions)
                .SingleAsync(x => x.Id == manyEntityOne.Id);

            Assert.That(manyEntityOneFromDb.ManyCompositions, Has.Count.EqualTo(0));
        }
    }

    [Test]
    public async Task _02_ExistingManyToManyRelationship_TrackedInComposition_ThrowsNoExceptionWhenSaving()
    {
        var manyItem = new ManyEntityOne
        {
            ManyCompositions = new List<ManyEntityTwo>
            {
                new()
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(manyItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdate = new ManyEntityOne
        {
            Id = manyItem.Id,
            ManyCompositions = new List<ManyEntityTwo>
            {
                new()
                {
                    Id = manyItem.ManyCompositions[0].Id
                }
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItemUpdate);
            Assert.DoesNotThrowAsync(async () => await dbContext.SaveChangesAsync());
        }
    }

    [Test]
    public async Task _03_ExistingManyToManyRelationship_TrackedInAggregation_ThrowsNoExceptionWhenSaving()
    {
        var manyItem = new ManyEntityOneAggregation
        {
            ManyAggregations = new List<ManyEntityTwoAggregation>
            {
                new()
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(manyItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdate = new ManyEntityOneAggregation
        {
            Id = manyItem.Id,
            ManyAggregations = new List<ManyEntityTwoAggregation>
            {
                new()
                {
                    Id = manyItem.ManyAggregations[0].Id
                }
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItemUpdate);
            Assert.DoesNotThrowAsync(async () => await dbContext.SaveChangesAsync());
        }
    }


    [Test]
    public async Task _04_AddingItemThatExistsInDbTo_ForceAggregation_ManyToManyCollection_AddsNewRelationship()
    {
        var manyItem = new ManyEntityOneAggregation();

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(manyItem);
            await dbContext.SaveChangesAsync();
        }

        var aggregationItemOne = new ManyEntityTwoAggregation();

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(aggregationItemOne);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdate = new ManyEntityOneAggregation
        {
            Id = manyItem.Id,
            ManyAggregations = new List<ManyEntityTwoAggregation>
            {
                new()
                {
                    Id = aggregationItemOne.Id
                }
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItemUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var manyItemFromDb = await dbContext.Set<ManyEntityOneAggregation>()
                .Include(x => x.ManyAggregations)
                .SingleAsync(x => x.Id == manyItem.Id);

            Assert.That(manyItemFromDb.ManyAggregations, Has.Count.EqualTo(1));
        }

        var aggregationItemTwo = new ManyEntityTwoAggregation();

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(aggregationItemTwo);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdateTwo = new ManyEntityOneAggregation
        {
            Id = manyItem.Id,
            ManyAggregations = new List<ManyEntityTwoAggregation>
            {
                new ManyEntityTwoAggregation
                {
                    Id = aggregationItemOne.Id
                },
                new ManyEntityTwoAggregation
                {
                    Id = aggregationItemTwo.Id
                }
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItemUpdateTwo);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var manyItemFromDb = await dbContext.Set<ManyEntityOneAggregation>()
                .Include(x => x.ManyAggregations)
                .SingleAsync(x => x.Id == manyItem.Id);

            Assert.That(manyItemFromDb.ManyAggregations, Has.Count.EqualTo(2));
        }
    }

    [Test]
    public async Task _05_AddingItemThatExistsInDbTo_Composition_ManyToManyCollection_AddsNewRelationship()
    {
        var manyItem = new ManyEntityOne();

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(manyItem);
            await dbContext.SaveChangesAsync();
        }

        var compositionItemOne = new ManyEntityTwo();

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(compositionItemOne);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdate = new ManyEntityOne
        {
            Id = manyItem.Id,
            ManyCompositions = new List<ManyEntityTwo>
            {
                new()
                {
                    Id = compositionItemOne.Id
                }
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItemUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var manyItemFromDb = await dbContext.Set<ManyEntityOne>()
                .Include(x => x.ManyCompositions)
                .SingleAsync(x => x.Id == manyItem.Id);

            Assert.That(manyItemFromDb.ManyCompositions, Has.Count.EqualTo(1));
        }

        var compositionItemTwo = new ManyEntityTwo();

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(compositionItemTwo);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdateTwo = new ManyEntityOne
        {
            Id = manyItem.Id,
            ManyCompositions = new List<ManyEntityTwo>
            {
                new ManyEntityTwo
                {
                    Id = compositionItemOne.Id
                },
                new ManyEntityTwo
                {
                    Id = compositionItemTwo.Id
                }
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItemUpdateTwo);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var manyItemFromDb = await dbContext.Set<ManyEntityOne>()
                .Include(x => x.ManyCompositions)
                .SingleAsync(x => x.Id == manyItem.Id);

            Assert.That(manyItemFromDb.ManyCompositions, Has.Count.EqualTo(2));
        }
    }
}