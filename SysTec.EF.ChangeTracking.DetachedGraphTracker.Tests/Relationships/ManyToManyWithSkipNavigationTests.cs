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
            EntityOneAssociations = new List<ManyEntityOne>()
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
    public async Task _03_ExistingManyToManyRelationship_TrackedInAssociation_ThrowsNoExceptionWhenSaving()
    {
        var manyItem = new ManyEntityOneAssociation
        {
            ManyAssociations = new List<ManyEntityTwoAssociation>
            {
                new()
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(manyItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdate = new ManyEntityOneAssociation
        {
            Id = manyItem.Id,
            ManyAssociations = new List<ManyEntityTwoAssociation>
            {
                new()
                {
                    Id = manyItem.ManyAssociations[0].Id
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
    public async Task _04_AddingItemThatExistsInDbTo_Association_ManyToManyCollection_AddsNewRelationship()
    {
        var manyItem = new ManyEntityOneAssociation();

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(manyItem);
            await dbContext.SaveChangesAsync();
        }

        var associationItemOne = new ManyEntityTwoAssociation();

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(associationItemOne);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdate = new ManyEntityOneAssociation
        {
            Id = manyItem.Id,
            ManyAssociations = new List<ManyEntityTwoAssociation>
            {
                new()
                {
                    Id = associationItemOne.Id
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
            var manyItemFromDb = await dbContext.Set<ManyEntityOneAssociation>()
                .Include(x => x.ManyAssociations)
                .SingleAsync(x => x.Id == manyItem.Id);

            Assert.That(manyItemFromDb.ManyAssociations, Has.Count.EqualTo(1));
        }

        var associationItemTwo = new ManyEntityTwoAssociation();

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(associationItemTwo);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdateTwo = new ManyEntityOneAssociation
        {
            Id = manyItem.Id,
            ManyAssociations = new List<ManyEntityTwoAssociation>
            {
                new ManyEntityTwoAssociation
                {
                    Id = associationItemOne.Id
                },
                new ManyEntityTwoAssociation
                {
                    Id = associationItemTwo.Id
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
            var manyItemFromDb = await dbContext.Set<ManyEntityOneAssociation>()
                .Include(x => x.ManyAssociations)
                .SingleAsync(x => x.Id == manyItem.Id);

            Assert.That(manyItemFromDb.ManyAssociations, Has.Count.EqualTo(2));
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