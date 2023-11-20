using SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Unit;

public class EqualityHelperTests
{

    public class EqualityWrapper
    {
        public Dictionary<string, object> Dict1 { get; set; }
        
        public Dictionary<string, object> Dict2 { get; set; }
    }

    private static readonly string _guid1 = Guid.NewGuid().ToString();
    
    private static readonly string _guid2 = Guid.NewGuid().ToString();

    private static readonly EqualityWrapper[] dictionaries = {
        new()
        {
            Dict1 = new()
            {
                { "Id_1", 1 },
                { "Id_2", 2 }
            },
            Dict2 = new()
            {
                { "Id_1", 1 },
                { "Id_2", 2 }
            }
        },
        new()
        {
            Dict1 = new()
            {
                { "Id_1", (long)1 },
                { "Id_2", (long)2 }
            },
            Dict2 = new()
            {
                { "Id_1", (long)1 },
                { "Id_2", (long)2 }
            }
        },
        new()
        {
            Dict1 = new()
            {
                { "Id_1", "Id_1_Val" },
                { "Id_2", "Id_2_Val" }
            },
            Dict2 = new()
            {
                { "Id_1", "Id_1_Val" },
                { "Id_2", "Id_2_Val" }
            }
        },
        new()
        {
            Dict1 = new()
            {
                { "Id_1", Guid.Parse(_guid1) },
                { "Id_2", Guid.Parse(_guid2) }
            },
            Dict2 = new()
            {
                { "Id_1", Guid.Parse(_guid1) },
                { "Id_2", Guid.Parse(_guid2) }
            }
        },
        new()
        {
            Dict1 = new()
            {
                { "Id_1", new byte[] { 1, 2, 3 } },
                { "Id_2", new byte[] { 4, 5, 6 } }
            },
            Dict2 = new()
            {
                { "Id_1", new byte[] { 1, 2, 3 } },
                { "Id_2", new byte[] { 4, 5, 6 } }
            }
        },
    };

    [Test]
    [TestCaseSource(nameof(dictionaries))]
    public void _Test_EqualityHelper_DoesNotCompareInstances(EqualityWrapper equalityWrapper)
    {
        Assert.That(equalityWrapper.Dict1, Is.Not.SameAs(equalityWrapper.Dict2));
    
        Assert.That(EqualityHelper.KeysAreEqual(equalityWrapper.Dict1, equalityWrapper.Dict2), Is.True);
    }
}