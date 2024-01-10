using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ChecklistTracker.LogicProvider.DataFiles.Settings
{
    /// <summary>
    /// https://stackoverflow.com/questions/20544641/how-to-perform-thread-safe-function-memoization-in-c
    /// </summary>
    internal class MemoizationCache
    {
        private IDictionary<string, IDictionary> Cache = new Dictionary<string, IDictionary>();

        public void Clear()
        {
            foreach (var item in Cache)
            {
                item.Value.Clear();
            }
        }

        public Func<TArgument, TResult> Memoize<TArgument, TResult>(string name, Func<TArgument, TResult> func)
        {
            var cache = new ConcurrentDictionary<TArgument, TResult>();
            var reentranceCheck = new ConcurrentDictionary<TArgument, bool>();

            Cache[name] = cache;
            Cache[name + "_entered"] = reentranceCheck;

            return (arg1) =>
            {
                return cache.GetOrAdd(arg1, tuple =>
                {
                    if (reentranceCheck.ContainsKey(arg1))
                    {
                        throw new InvalidOperationException($"Reentrance {tuple}");
                    }
                    reentranceCheck[arg1] = true;
                    return func(arg1);
                });
            };
        }

        public Func<TArgument1, TArgument2, TResult> Memoize<TArgument1, TArgument2, TResult>(string name, Func<TArgument1, TArgument2, TResult> func)
        {
            var cache = new ConcurrentDictionary<(TArgument1, TArgument2), TResult>();
            var reentranceCheck = new ConcurrentDictionary<(TArgument1, TArgument2), bool>();

            Cache[name] = cache;
            Cache[name + "_entered"] = reentranceCheck;

            return (arg1, arg2) =>
            {
                return cache.GetOrAdd((arg1, arg2), tuple =>
                {
                    if (reentranceCheck.ContainsKey((arg1, arg2)))
                    {
                        throw new InvalidOperationException($"Reentrance {tuple}");
                    }
                    reentranceCheck[(arg1, arg2)] = true;
                    return func(tuple.Item1, tuple.Item2);
                });
            };
        }

        public Func<TArgument1, TArgument2, TArgument3, TArgument4, TResult>
            Memoize<TArgument1, TArgument2, TArgument3, TArgument4, TResult>(string name, Func<TArgument1, TArgument2, TArgument3, TArgument4, TResult> func)
        {
            var cache = new ConcurrentDictionary<(TArgument1, TArgument2, TArgument3, TArgument4), TResult>();
            var reentranceCheck = new ConcurrentDictionary<(TArgument1, TArgument2, TArgument3, TArgument4), bool>();

            Cache[name] = cache;
            Cache[name + "_entered"] = reentranceCheck;

            return (arg1, arg2, arg3, arg4) =>
            {
                return cache.GetOrAdd((arg1, arg2, arg3, arg4), tuple =>
                {
                    if (reentranceCheck.ContainsKey((arg1, arg2, arg3, arg4)))
                    {
                        throw new InvalidOperationException($"Reentrance {tuple}");
                    }
                    reentranceCheck[(arg1, arg2, arg3, arg4)] = true;
                    return func(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
                });
            };
        }

        public Func<TArgument1, TArgument2, bool, TResult> Memoize<TArgument1, TArgument2, TResult>(string name, Func<TArgument1, TArgument2, TResult> func, Func<TResult> onReentrance)
        {
            var cache = new ConcurrentDictionary<(TArgument1, TArgument2), TResult>();
            var reentranceCheck = new ConcurrentDictionary<(TArgument1, TArgument2), bool>();

            Cache[name] = cache;
            Cache[name + "_entered"] = reentranceCheck;

            return (arg1, arg2, allowReentrance) =>
            {
                return cache.GetOrAdd((arg1, arg2), tuple =>
                {
                    if (reentranceCheck.ContainsKey((arg1, arg2)))
                    {
                        if (allowReentrance)
                        {
                            return onReentrance();
                        }
                        throw new InvalidOperationException("Reentrance");
                    }
                    reentranceCheck[(arg1, arg2)] = true;
                    return func(tuple.Item1, tuple.Item2);
                });
            };
        }

        public Func<TArgument1, TArgument2, TArgument3, bool, TResult> Memoize<TArgument1, TArgument2, TArgument3, TResult>(string name, Func<TArgument1, TArgument2, TArgument3, TResult> func, Func<TResult> onReentrance)
        {
            var cache = new ConcurrentDictionary<(TArgument1, TArgument2, TArgument3), TResult>();
            var reentranceCheck = new ConcurrentDictionary<(TArgument1, TArgument2, TArgument3), bool>();

            Cache[name] = cache;
            Cache[name + "_entered"] = reentranceCheck;

            return (arg1, arg2, arg3, allowReentrance) =>
            {
                return cache.GetOrAdd((arg1, arg2, arg3), tuple =>
                {
                    if (reentranceCheck.ContainsKey((arg1, arg2, arg3)))
                    {
                        if (allowReentrance)
                        {
                            return onReentrance();
                        }
                        throw new InvalidOperationException("Reentrance");
                    }
                    reentranceCheck[(arg1, arg2, arg3)] = true;
                    return func(tuple.Item1, tuple.Item2, tuple.Item3);
                });
            };
        }
    }
}
