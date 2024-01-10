using System.Collections.Concurrent;
using System.Collections.Generic;
using Windows.Gaming.Input;

namespace ChecklistTracker.LogicProvider
{
    internal static class DictionaryExtensions
    {
        public static void PutOrAdd<TKey, TValue>(this ConcurrentDictionary<TKey, ISet<TValue>> dict, TKey key, TValue newValue)
            where TKey : notnull
        {
            dict.GetOrAdd(key, (key) => new HashSet<TValue>()).Add(newValue);
        }
        public static void PutOrAdd<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key, TValue newValue)
            where TKey : notnull
        {
            dict.GetOrAdd(key, (key) => newValue);
        }
    }
}
