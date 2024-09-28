using System;
using System.Collections.Generic;
using System.Linq;

namespace ChecklistTracker.LogicProvider
{
    [Flags]
    public enum Accessibility
    {
        None = 0,
        Peekable = 1,
        Accessible = 2,
        SequenceBreak = Accessible | Peekable,
        Synthetic = 4,
        SyntheticAssumed = Synthetic | Accessible | Peekable,
        InLogic = 8,
        All = InLogic | SyntheticAssumed
    }

    public static class AccessibilityExtensions
    {
        public static IEnumerable<Accessibility> Flags = new Accessibility[]
        {
            Accessibility.InLogic,
            Accessibility.Synthetic,
            Accessibility.Accessible,
            Accessibility.Peekable,
        };

        public static Accessibility ToAccessibility(this bool value)
        {
            return value ? Accessibility.All : Accessibility.None;
        }

        public static Accessibility Or(params Func<Accessibility>[] evals)
        {
            return evals.Or();
        }

        public static Accessibility Or(this IEnumerable<Accessibility> list)
        {
            return list.Or(a => a);
        }

        public static Accessibility Or<T>(this IEnumerable<T> list, Func<T, Accessibility> func)
        {
            return list.Select(item => (Func<Accessibility>)(() => func(item))).Or();
        }

        public static Accessibility Or(this IEnumerable<Func<Accessibility>> list)
        {
            if (list == null || list.Count() == 0)
            {
                return Accessibility.None;
            }
            var accessibility = Accessibility.None;
            foreach (var item in list)
            {
                accessibility |= item();
                if (accessibility >= Accessibility.All)
                {
                    break;
                }
            }
            return accessibility;
        }

        public static Accessibility And(params Func<Accessibility>[] evals)
        {
            return evals.And();
        }

        public static Accessibility And(this IEnumerable<Accessibility> list)
        {
            return list.And(a => a);
        }

        public static Accessibility And<T>(this IEnumerable<T> list, Func<T, Accessibility> func)
        {
            return list.Select(item => (Func<Accessibility>)(() => func(item))).And();
        }

        public static Accessibility And(this IEnumerable<Func<Accessibility>> list)
        {
            if (list == null || list.Count() == 0)
            {
                return Accessibility.All;
            }
            var accessibility = Accessibility.All;
            foreach (var item in list)
            {
                accessibility &= item();
                if (accessibility <= Accessibility.None)
                {
                    break;
                }
            }
            return accessibility;
        }

        public static Accessibility MostAccessible<T>(this IEnumerable<T> items, Func<T, Accessibility> eval, int minCount)
        {
            return items.Select(item => (Func<Accessibility>)(() => eval(item))).MostAccessible(minCount);
        }

        public static Accessibility MostAccessible(this IEnumerable<Func<Accessibility>> funcs, int minCount)
        {
            var flagCount = new Dictionary<Accessibility, int>();
            var count = 0;

            foreach (var func in funcs)
            {
                var accessibility = func();

                foreach (var flag in Flags)
                {
                    if (accessibility.HasFlag(flag))
                    {
                        if (!flagCount.ContainsKey(flag))
                        {
                            flagCount[flag] = 0;
                        }
                        flagCount[flag]++;
                    }
                }

                if (flagCount.TryGetValue(Accessibility.InLogic, out count) && count > minCount)
                {
                    break;
                }
            }

            if (flagCount.TryGetValue(Accessibility.InLogic, out count) && count > minCount)
            {
                return Accessibility.All;
            }
            if (flagCount.TryGetValue(Accessibility.Synthetic, out count) && count > minCount)
            {
                return Accessibility.SyntheticAssumed;
            }
            if (flagCount.TryGetValue(Accessibility.Accessible, out count) && count > minCount)
            {
                return Accessibility.SequenceBreak;
            }
            if (flagCount.TryGetValue(Accessibility.Peekable, out count) && count > minCount)
            {
                return Accessibility.Peekable;
            }
            return Accessibility.None;
        }
    }
}
