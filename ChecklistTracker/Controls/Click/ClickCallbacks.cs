using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.ApplicationModel.DataTransfer;

namespace ChecklistTracker.Controls.Click
{
    internal class ClickCallbacks
    {
        internal delegate void ClickHandler(UIElement sender, MouseButton button);
        internal delegate void ItemClickHandler(UIElement sender, object item, MouseButton button);

        internal delegate void ScrollHandler(UIElement sender, int scrollAmount);

        internal ClickHandler? OnClick;
        internal ItemClickHandler? OnItemClick;
        internal ScrollHandler? OnScroll;

        internal IDragProvider<ImageSource>? DragImageProvider;
        internal IDropProvider<ImageSource>? DropImageProvider;
        internal IDragProvider<HintControl>? DragHintControlProvider;
        internal IDropProvider<HintControl>? DropHintControlProvider;
    }

    internal interface IDragProvider<T>
    {
        T GetDragData(MouseButton dragType);
        void OnDataDraggedFrom(MouseButton dragType);
    }

    internal interface IDropProvider<T>
    {
        void OnDataDroppedTo(T data);
    }
}
