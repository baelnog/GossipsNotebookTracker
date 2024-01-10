using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistTracker.CoreUtils
{
    internal class OnDispose : IDisposable
    {
        private Action DisposeAction;
        public OnDispose(Action onDispose)
        {
            DisposeAction = onDispose;
        }

        public void Dispose()
        {
            DisposeAction();
        }
    }
}
