using Microsoft.UI.Xaml;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;

namespace ChecklistTracker.Controls.Click
{
    internal static partial class ClickTracker
    {
        private class DragInfo
        {
            internal MouseButton Button;
            internal UIElement Source;
            internal UIElement? CurrentTarget = null;

            internal DragInfo(UIElement source, MouseButton button)
            {
                Button = button;
                Source = source;
            }
        }

    }
}
