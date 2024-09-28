using System;

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
