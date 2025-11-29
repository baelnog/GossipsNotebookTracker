using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.ApplicationModel.DataTransfer;

namespace ChecklistTracker.Controls.Click
{
    internal class ClickCallbacks
    {
        internal delegate void ClickHandler(UIElement sender, MouseButton button);

        //internal delegate ImageSource GetImageDragData();
        //internal delegate HintControl GetHintDragData();

        //internal delegate ImageSource DragImageCompletedHandler(UIElement sender, MouseButton button);
        //internal delegate void DropImageCompletedHandler(UIElement sender, MouseButton button, ImageSource draggedImage);

        //internal delegate HintControl DragHintControlCompletedHandler(UIElement sender, MouseButton button);
        //internal delegate void DropHintControlCompletedHandler(UIElement sender, MouseButton button, HintControl draggedImage);

        internal delegate void ScrollHandler(UIElement sender, int scrollAmount);

        internal ClickHandler? OnClick;
        //internal DragImageCompletedHandler? OnDragImageCompleted;
        //internal DropImageCompletedHandler? OnDropImageCompleted;
        internal ScrollHandler? OnScroll;

        //internal GetImageDragData? OnGetImageDragData;
        //internal GetHintDragData? OnGetHintDragData;

        //internal DragHintControlCompletedHandler? OnDragHintControlCompleted;
        //internal DropHintControlCompletedHandler? OnDropHintControlCompleted;

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
