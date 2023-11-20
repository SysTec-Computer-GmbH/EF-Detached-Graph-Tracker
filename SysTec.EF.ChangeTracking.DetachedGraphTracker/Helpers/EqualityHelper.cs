using System.Collections;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

internal static class EqualityHelper
{
    internal static bool KeysAreEqual(Dictionary<string, object> dictionary1, Dictionary<string, object> dictionary2)
    {
        if (dictionary1.Count != dictionary2.Count) return false;
        var equal = true;
        foreach (var pair in dictionary1)
        {
            if (dictionary2.TryGetValue(pair.Key, out var value))
            {
                var valuesAreEqual = IsValueEqual(value, pair.Value);
                if (valuesAreEqual) continue;
                equal = false;
                break;
            }

            equal = false;
            break;
        }

        return equal;
    }

    private static bool IsValueEqual(object value1, object value2)
    {
        if (value1.GetType().IsArray)
        {
            return ArraysAreEqual(ConvertObjectToEnumerable(value1),ConvertObjectToEnumerable(value2));
        }
        
        return value1.Equals(value2);
    }

    private static bool ArraysAreEqual(IEnumerable<object> arr1, IEnumerable<object> arr2)
    {
        return arr1.SequenceEqual(arr2);
    }
    
    private static IEnumerable<object> ConvertObjectToEnumerable(object obj)
    {
        return ((IEnumerable)obj).Cast<object>();
    }
}