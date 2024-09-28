using System;
using System.Collections;
using System.Collections.Generic;

namespace ChecklistTracker.CoreUtils
{
    public class FuncComparer : FuncComparer<object>, IComparer
    {
        public FuncComparer(Func<object?, object?, int> compare) : base(compare) { }
    }

    public class FuncComparer<T> : IComparer<T>
    {
        private readonly Func<T?, T?, int> Comparer;

        public FuncComparer(Func<T?, T?, int> compare)
        {
            Comparer = compare;
        }

        public int Compare(T? x, T? y)
        {
            return Comparer(x, y);
        }
    }
}
