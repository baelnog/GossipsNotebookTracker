using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistTracker.LogicProvider
{
    internal class DoubleConcurrentDictionary<TValue> : ConcurrentDictionary<string, ConcurrentDictionary<string, TValue>>
    {
        public ConcurrentDictionary<string, TValue> GetOrNew(string key)
        {
            return GetOrAdd(key, (key) => new ConcurrentDictionary<string, TValue>());
        }
    }

    internal class TripleConcurrentDictionary<TValue> : ConcurrentDictionary<string, DoubleConcurrentDictionary<TValue>>
    {
        public DoubleConcurrentDictionary<TValue> GetOrNew(string key)
        {
            return GetOrAdd(key, (key) => new DoubleConcurrentDictionary<TValue>());
        }
    }
}
