using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace ChecklistTracker.Controls.Click
{
    internal class ClickCallbacks
    {
        internal delegate void ClickHandler(UIElement sender, MouseButton button);
        internal delegate ImageSource DragImageCompletedHandler(UIElement sender, MouseButton button);
        internal delegate void DropImageCompletedHandler(UIElement sender, MouseButton button, ImageSource draggedImage);

        internal delegate HintControl DragHintControlCompletedHandler(UIElement sender, MouseButton button);
        internal delegate void DropHintControlCompletedHandler(UIElement sender, MouseButton button, HintControl draggedImage);

        internal delegate void ScrollHandler(UIElement sender, int scrollAmount);

        internal ClickHandler? OnClick;
        internal DragImageCompletedHandler? OnDragImageCompleted;
        internal DropImageCompletedHandler? OnDropImageCompleted;
        internal ScrollHandler? OnScroll;

        internal DragHintControlCompletedHandler? OnDragHintControlCompleted;
        internal DropHintControlCompletedHandler? OnDropHintControlCompleted;
    }
}
