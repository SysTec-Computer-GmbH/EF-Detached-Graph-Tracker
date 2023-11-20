using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.CompositeKeys.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.CompositeKeys.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.CompositeKeys;

public class CompositeKeyTests : TestBase<CompositeKeyTestsDbContext>
{
    private const int ID_VALUE = 1;
    private const int KEY2_VALUE = 42;

    [Test]
    public async Task _01_EntitiesWithCompositeKeys_CanBeStoredAndUpdated_UsingTrackGraphHandler()
    {
        var compositeKeyEntity = GetCompositeKeyEntity();

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(compositeKeyEntity);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var compositeKeyEntityFromDb = await dbContext.CompositeKeyEntities.SingleAsync();
            Assert.Multiple(() =>
            {
                Assert.That(compositeKeyEntityFromDb.Id, Is.EqualTo(ID_VALUE));
                Assert.That(compositeKeyEntityFromDb.Key2, Is.EqualTo(KEY2_VALUE));
            });
        }

        const string updatedText = "Updated Composite Key Item";
        var compositeKeyEntityUpdate = (CompositeKeyEntity)compositeKeyEntity.Clone();
        compositeKeyEntityUpdate.Text = updatedText;

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(compositeKeyEntityUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var compositeKeyEntityFromDb = await dbContext.CompositeKeyEntities.SingleAsync();
            Assert.That(compositeKeyEntityFromDb.Text, Is.EqualTo(updatedText));
        }
    }

    [Test]
    public async Task
        _02_RelationshipsWithCompositeKeyEntities_ForCompositions_CanBeConnectedAndSevered_UsingTrackGraphHandler()
    {
        var compositeKeyEntity = GetCompositeKeyEntity();
        compositeKeyEntity.A_Composition_Item = new CompositeForeignKeyCompositionEntity
        {
            Text = "Reference Composition Item"
        };

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(compositeKeyEntity);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var compositeKeyEntityFromDb = await dbContext.CompositeKeyEntities
                .Include(x => x.A_Composition_Item)
                .SingleAsync();

            Assert.That(compositeKeyEntityFromDb.A_Composition_Item, Is.Not.Null);
        }

        var compositeKeyEntityInvalidUpdate = (CompositeKeyEntity)compositeKeyEntity.Clone();
        compositeKeyEntityInvalidUpdate.B_Composition_Items.Add(
            (CompositeForeignKeyCompositionEntity)compositeKeyEntityInvalidUpdate.A_Composition_Item!.Clone());

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.ThrowsAsync<MultipleCompositionException>(async () =>
                 await graphTracker.TrackGraphAsync(compositeKeyEntityInvalidUpdate));
        }

        const string updatedText = "Updated Text";
        var compositeKeyEntityUpdate = (CompositeKeyEntity)compositeKeyEntity.Clone();
        compositeKeyEntityUpdate.B_Composition_Items.Add(compositeKeyEntityUpdate.A_Composition_Item!);
        compositeKeyEntityUpdate.B_Composition_Items[0].Text = updatedText;
        compositeKeyEntityUpdate.A_Composition_Item = null;

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(compositeKeyEntityUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var compositeKeyEntityFromDb = await dbContext.CompositeKeyEntities
                .Include(x => x.A_Composition_Item)
                .Include(x => x.B_Composition_Items)
                .SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(compositeKeyEntityFromDb.A_Composition_Item, Is.Null);
                Assert.That(compositeKeyEntityFromDb.B_Composition_Items, Has.Count.EqualTo(1));
                Assert.That(compositeKeyEntityFromDb.B_Composition_Items[0].Text, Is.EqualTo(updatedText));
            });
        }

        var compositeKeyEntityUpdate2 = (CompositeKeyEntity)compositeKeyEntityUpdate.Clone();
        compositeKeyEntityUpdate2.B_Composition_Items.Clear();

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(compositeKeyEntityUpdate2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var compositeKeyEntityFromDb = await dbContext.CompositeKeyEntities
                .Include(x => x.A_Composition_Item)
                .Include(x => x.B_Composition_Items)
                .SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(compositeKeyEntityFromDb.A_Composition_Item, Is.Null);
                Assert.That(compositeKeyEntityFromDb.B_Composition_Items, Is.Empty);
            });
        }
    }

    [Test]
    public async Task
        _03_RelationshipsWithCompositeKeyEntities_ForAssociations_CanBeConnectedAndSevered_UsingTrackGraphHandler()
    {
        const string originalText = "Reference Association Item";
        var associationItem = new CompositeForeignKeyAssociationEntity
        {
            Text = originalText
        };

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            dbContext.Add(associationItem);
            await dbContext.SaveChangesAsync();
        }

        var compositeKeyEntity = GetCompositeKeyEntity();
        compositeKeyEntity.A_Association_Item = (CompositeForeignKeyAssociationEntity)associationItem.Clone();

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(compositeKeyEntity);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var compositeKeyEntityFromDb = await dbContext.CompositeKeyEntities
                .Include(x => x.A_Association_Item)
                .SingleAsync();

            Assert.That(compositeKeyEntityFromDb.A_Association_Item, Is.Not.Null);
        }

        const string updatedText = "Updated Text";
        var compositeKeyEntityUpdate = (CompositeKeyEntity)compositeKeyEntity.Clone();
        compositeKeyEntityUpdate.B_Association_Items.Add(
            (CompositeForeignKeyAssociationEntity)compositeKeyEntityUpdate.A_Association_Item!.Clone());
        compositeKeyEntityUpdate.B_Association_Items[0].Text = updatedText;

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(compositeKeyEntityUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var compositeKeyEntityFromDb = await dbContext.CompositeKeyEntities
                .Include(x => x.A_Association_Item)
                .Include(x => x.B_Association_Items)
                .SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(compositeKeyEntityFromDb.A_Association_Item, Is.Not.Null);
                Assert.That(compositeKeyEntityFromDb.A_Association_Item!.Id, Is.EqualTo(associationItem.Id));
                Assert.That(compositeKeyEntityFromDb.A_Association_Item!.Text, Is.EqualTo(originalText));
                Assert.That(compositeKeyEntityFromDb.B_Association_Items, Has.Count.EqualTo(1));
                Assert.That(compositeKeyEntityFromDb.B_Association_Items[0].Id, Is.EqualTo(associationItem.Id));
                Assert.That(compositeKeyEntityFromDb.B_Association_Items[0].Text, Is.EqualTo(originalText));
            });
        }

        var compositeKeyEntityUpdate2 = (CompositeKeyEntity)compositeKeyEntityUpdate.Clone();
        compositeKeyEntityUpdate2.B_Association_Items.Clear();

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(compositeKeyEntityUpdate2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var compositeKeyEntityFromDb = await dbContext.CompositeKeyEntities
                .Include(x => x.A_Association_Item)
                .Include(x => x.B_Association_Items)
                .SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(compositeKeyEntityFromDb.A_Association_Item, Is.Not.Null);
                Assert.That(compositeKeyEntityFromDb.A_Association_Item!.Id, Is.EqualTo(associationItem.Id));
                Assert.That(compositeKeyEntityFromDb.B_Association_Items, Is.Empty);
            });
        }

        var compositeKeyEntityUpdate3 = (CompositeKeyEntity)compositeKeyEntityUpdate2.Clone();
        compositeKeyEntityUpdate3.A_Association_Item = null;

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(compositeKeyEntityUpdate3);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new CompositeKeyTestsDbContext())
        {
            var compositeKeyEntityFromDb = await dbContext.CompositeKeyEntities
                .Include(x => x.A_Association_Item)
                .Include(x => x.B_Association_Items)
                .SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(compositeKeyEntityFromDb.A_Association_Item, Is.Null);
                Assert.That(compositeKeyEntityFromDb.B_Association_Items, Is.Empty);
            });
        }
    }

    private CompositeKeyEntity GetCompositeKeyEntity()
    {
        return new CompositeKeyEntity
        {
            Id = ID_VALUE,
            Key2 = KEY2_VALUE,
            Text = "Composite Key Item"
        };
    }
}