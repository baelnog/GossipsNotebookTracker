using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistTracker.CoreUtils;

public class DoubleConcurrentDictionary<TKey1, TKey2, TValue> :
    ConcurrentDictionary<TKey1, ConcurrentDictionary<TKey2, TValue>>
    where TKey1 : notnull
    where TKey2 : notnull
{
    public ConcurrentDictionary<TKey2, TValue> GetOrNew(TKey1 key)
    {
        return GetOrAdd(key, (key) => new ConcurrentDictionary<TKey2, TValue>());
    }
}

public class TripleConcurrentDictionary<TKey1, TKey2, TKey3, TValue> :
    ConcurrentDictionary<TKey1, DoubleConcurrentDictionary<TKey2, TKey3, TValue>>
    where TKey1 : notnull
    where TKey2 : notnull
    where TKey3 : notnull
{
    public DoubleConcurrentDictionary<TKey2, TKey3, TValue> GetOrNew(TKey1 key)
    {
        return GetOrAdd(key, (key) => new DoubleConcurrentDictionary<TKey2, TKey3, TValue>());
    }
}
