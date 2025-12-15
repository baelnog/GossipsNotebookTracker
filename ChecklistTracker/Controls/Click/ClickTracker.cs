using ChecklistTracker.CoreUtils;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;

namespace ChecklistTracker.Controls.Click
{
    internal static partial class ClickTracker
    {
        internal static void ConfigureClickHandler(this UIElement source, ClickCallbacks callbacks)
        {
            if (callbacks.OnClick != null)
            {
                //source.RightTapped += (s, e) => callbacks.OnClick(source, MouseButton.Right);
                source.IsDoubleTapEnabled = false;
                source.PointerPressed += (s, e) => Click_OnPointerPressed(callbacks.OnClick, source, e);
            }

            if (callbacks.DragHintControlProvider != null || callbacks.DragImageProvider != null)
            {
                source.CanDrag = true;
                source.PointerPressed += (s, e) => Drag_OnPointerPressed(callbacks, source, e);
            }

            if (callbacks.DropHintControlProvider != null || callbacks.DropImageProvider != null)
            {
                source.AllowDrop = true;
                source.DragOver += (s, e) => OnDragOver(callbacks, source, e);
                source.Drop += (s, e) => OnDrop(callbacks, source, e);
            }

            if (callbacks.OnScroll != null)
            {
                source.PointerWheelChanged += (object s, PointerRoutedEventArgs e) => OnPointerWheelChanged(callbacks.OnScroll, source, e);
            }
        }

        static string GetDescription(UIElement elem)
        {
            if (elem is Image image)
            {
                if (image.Source is BitmapImage bitmapImage)
                {
                    var uri = bitmapImage.UriSource;
                    var filePart = uri.PathAndQuery.Split("/").Last();
                    return $"Image: {filePart}";
                }
                return "Image";
            }
            return elem?.ToString() ?? "<null>";
        }

        static MouseButton? GetButton(PointerPoint pointer)
        {
            var props = pointer.Properties;
            switch (props.PointerUpdateKind)
            {
                case PointerUpdateKind.LeftButtonReleased:
                case PointerUpdateKind.LeftButtonPressed:
                    return MouseButton.Left;
                case PointerUpdateKind.RightButtonReleased:
                case PointerUpdateKind.RightButtonPressed:
                    return MouseButton.Right;
                case PointerUpdateKind.MiddleButtonReleased:
                case PointerUpdateKind.MiddleButtonPressed:
                    return MouseButton.Middle;
                default:
                    return null;
            }
        }

        private static void Click_OnPointerPressed(ClickCallbacks.ClickHandler onClick, UIElement source, PointerRoutedEventArgs e)
        {
            var button = GetButton(e.GetCurrentPoint(source));
            if (button.HasValue)
            {
                PointerEventHandler? onRelease = null;
                PointerEventHandler? onLeave = null;
                TypedEventHandler<UIElement, DragStartingEventArgs>? onDrag = null;
                onDrag = (UIElement s, DragStartingEventArgs e) =>
                {
                    source.DragStarting -= onDrag;
                    source.PointerReleased -= onRelease;
                    source.PointerExited -= onLeave;
                };

                onRelease = (object s, PointerRoutedEventArgs e) =>
                {
                    source.DragStarting -= onDrag;
                    source.PointerReleased -= onRelease;
                    source.PointerExited -= onLeave;

                    onClick(source, button.Value);
                };

                onLeave = (object s, PointerRoutedEventArgs e) =>
                {
                    source.DragStarting -= onDrag;
                    source.PointerReleased -= onRelease;
                    source.PointerExited -= onLeave;
                };
                source.DragStarting += onDrag;
                source.PointerReleased += onRelease;
                source.PointerExited += onLeave;
            }
        }

        private static void Drag_OnPointerPressed(ClickCallbacks callbacks, UIElement source, PointerRoutedEventArgs e)
        {
            var pointerPoint = e.GetCurrentPoint(source);
            var button = GetButton(pointerPoint);
            if (!button.HasValue)
            {
                Logging.WriteLine($"Drag_OnPointerPressed {GetDescription(source)} - no button?");
                return;
            }

            TypedEventHandler<UIElement, DragStartingEventArgs>? dragStarting = null;
            dragStarting = (s, e) =>
            {
                source.DragStarting -= dragStarting;
                Drag_OnDragStarting(callbacks, source, button.Value, e);
            };
            source.DragStarting += dragStarting;

            PointerEventHandler? onLeave = null;
            PointerEventHandler? onRelease = null;

            onRelease = (s, e) =>
            {
                source.PointerReleased -= onRelease;
                source.PointerExited -= onLeave;
                source.DragStarting -= dragStarting;
            };

            onLeave = async (s, e) =>
            {
                source.PointerReleased -= onRelease;
                source.PointerExited -= onLeave;

                if (button != MouseButton.Left)
                {
                    var result = await source.StartDragAsync(pointerPoint);
                }             
            };
            source.PointerExited += onLeave;
            source.PointerReleased += onRelease;
        }

        private static void Drag_OnDragStarting(ClickCallbacks callbacks, UIElement source, MouseButton button, DragStartingEventArgs e)
        {
            var imageSource = callbacks.DragImageProvider?.GetDragData(button);
            var hintSource = callbacks.DragHintControlProvider?.GetDragData(button);
            Logging.WriteLine($"Drag_OnDragStarting {GetDescription(source)}");

            var operation = GetDataPackageOperation(button);
            if (!operation.HasValue)
            {
                Logging.WriteLine($"Drag_OnDragStarting {GetDescription(source)} - no operation?");
                return;
            }
            e.AllowedOperations = operation.Value;

            e.Data.Properties["imageSource"] = imageSource;
            e.Data.Properties["hintControl"] = hintSource;

            TypedEventHandler<UIElement, DropCompletedEventArgs>? onDropped = null;
            onDropped = (s, e) =>
            {
                Logging.WriteLine($"DropCompleted for {GetDescription(source)} result: {e.DropResult}");
                source.DropCompleted -= onDropped;

                var result = e.DropResult;
                if (result == DataPackageOperation.None)
                {
                    Logging.WriteLine($"Drag_OnDragStarting OnDropped {GetDescription(source)} - no drop?");
                    return;
                }
                Logging.WriteLine($"Drag_OnDragStarting OnDropped {GetDescription(source)}");
                if (callbacks.DragImageProvider != null)
                {
                    callbacks.DragImageProvider.OnDataDraggedFrom(button);
                }
                if (callbacks.DragHintControlProvider != null && hintSource != null)
                {
                    callbacks.DragHintControlProvider.OnDataDraggedFrom(button);
                }
            };
            source.DropCompleted += onDropped;
        }

        static void OnDragOver(ClickCallbacks callback, UIElement target, DragEventArgs e)
        {
            Logging.WriteLine($"OnDragOver: {GetDescription(target)}");
            e.AcceptedOperation = DataPackageOperation.None;
            e.DragUIOverride.IsCaptionVisible = false;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = false;

            if (callback.DropHintControlProvider != null && e.DataView.Properties.ContainsKey("hintControl")
                || callback.DropImageProvider != null && e.DataView.Properties.ContainsKey("imageSource"))
            {
                e.AcceptedOperation = e.AllowedOperations;
            }

            Logging.WriteLine($"OnDragOver: {GetDescription(target)}: {e.AcceptedOperation}");
        }

        private static DataPackageOperation? GetDataPackageOperation(MouseButton button)
        {
            if (button.HasFlag(MouseButton.Left))
            {
                return DataPackageOperation.Copy;
            }
            if (button.HasFlag(MouseButton.Right))
            {
                return DataPackageOperation.Move;
            }
            return null;
        }

        private static async void OnDrop(ClickCallbacks callback, UIElement target, DragEventArgs e)
        {
            var element = target;

            Logging.WriteLine($"OnDrop {GetDescription(element)} {e.AllowedOperations} - operation");

            Logging.WriteLine($"hintControl: {e.DataView.Properties["hintControl"]}");
            Logging.WriteLine($"imageSource: {e.DataView.Properties["imageSource"]}");
            if (callback.DropHintControlProvider != null && e.DataView.Properties["hintControl"] is HintControl hintControlData)
            {
                Logging.WriteLine($"OnDrop invoking OnDropHintControlCompleted");
                callback.DropHintControlProvider.OnDataDroppedTo(hintControlData);
                e.DataView.ReportOperationCompleted(e.AllowedOperations);
                return;
            }
            if (callback.DropImageProvider != null && e.DataView.Properties["imageSource"] is ImageSource imageSourceData)
            {
                Logging.WriteLine($"OnDrop invoking OnDropImageCompleted");
                callback.DropImageProvider.OnDataDroppedTo(imageSourceData);
                e.DataView.ReportOperationCompleted(e.AllowedOperations);
                return;
            }
        }

        static void OnPointerWheelChanged(ClickCallbacks.ScrollHandler sourceCallback, UIElement uiElement, PointerRoutedEventArgs winArgs)
        {
            Logging.WriteLine($"OnPointerWheelChanged");
            Logging.WriteLine($"OnPointerWheelChanged has callback");
            var delta = winArgs.GetCurrentPoint(uiElement).Properties.MouseWheelDelta;
            var dir = Math.Sign(delta);
            sourceCallback(uiElement, dir);
            winArgs.Handled = true;
            Logging.WriteLine($"OnPointerWheelChanged Done");
        }
    }
}
