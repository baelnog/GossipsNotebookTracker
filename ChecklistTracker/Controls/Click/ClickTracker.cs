using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;

namespace ChecklistTracker.Controls.Click
{
    internal static partial class ClickTracker
    {

        private static Dictionary<MouseButton, UIElement> TrackedEvents = new Dictionary<MouseButton, UIElement>();

        private static DragInfo CurrentDrag = null;

        private static Dictionary<UIElement, ClickCallbacks> ClickCallbacks = new Dictionary<UIElement, ClickCallbacks>();

        internal static void ConfigureClickHandler(this UIElement source, ClickCallbacks callbacks)
        {
            ClickCallbacks[source] = callbacks;

            source.CanDrag = callbacks.OnDragImageCompleted != null || callbacks.OnDragHintControlCompleted != null;
            source.AllowDrop = callbacks.OnDropImageCompleted != null || callbacks.OnDropHintControlCompleted != null;
            source.DropCompleted += (s,e) => OnDropCompleted(source, s, e);
            source.PointerReleased += OnPointerReleased;
            source.PointerPressed += OnPointerPressed;
            source.PointerExited += OnPointerMoved;
            source.DragOver += OnDragOver;
            
            if (callbacks.OnScroll != null)
            {
                source.PointerWheelChanged += (object sender, PointerRoutedEventArgs args) => OnPointerWheelChanged(source, args);
            }
        }

        static void OnPointerPressed(object s, PointerRoutedEventArgs e)
        {
            if (s is UIElement sender)
            {
                var pointer = e.GetCurrentPoint((UIElement)e.OriginalSource);
                var props = pointer.Properties;
                MouseButton clickedButton;
                switch (props.PointerUpdateKind)
                {
                    case PointerUpdateKind.LeftButtonPressed:
                        clickedButton = MouseButton.Left;
                        break;
                    case PointerUpdateKind.RightButtonPressed:
                        clickedButton = MouseButton.Right;
                        break;
                    case PointerUpdateKind.MiddleButtonPressed:
                        clickedButton = MouseButton.Middle;
                        break;
                    default:
                        return;
                }
                TrackedEvents[clickedButton] = sender;
                e.Handled = true;
            }
        }

        static void OnPointerMoved(object s, PointerRoutedEventArgs e)
        {
            if (s is UIElement sender && e.OriginalSource is UIElement originalSource)
            {
                var pointer = e.GetCurrentPoint(originalSource);
                var props = pointer.Properties;

                MouseButton button;
                if (props.IsLeftButtonPressed)
                {
                    button = MouseButton.Left;
                }
                else if (props.IsRightButtonPressed)
                {
                    button = MouseButton.Right;
                }
                else
                {
                    return;
                }
                Debug.WriteLine($"OnPointerMoved {button}");
                Debug.WriteLine($"StartDrag {button}");
                StartDrag(button, sender, pointer);
            }
        }

        static void OnDragOver(object s, DragEventArgs e)
        {
            e.DragUIOverride.IsGlyphVisible = false;
            if (s is UIElement sender)
            {
                Debug.WriteLine($"OnDragOver pre lock");
                var drag = CurrentDrag;
                if (drag != null)
                {
                    Debug.WriteLine($"OnDragOver CurrentDrag.CurrentTarget = {sender}");
                    drag.CurrentTarget = sender;
                }
                e.Handled |= true;
            }
        }

        static void StartDrag(MouseButton button, UIElement source, PointerPoint pointer)
        {
            Debug.WriteLine($"StartDrag pre lock");
            Debug.WriteLine($"StartDrag");
            TrackedEvents.Remove(button);
            if (CurrentDrag?.Button != button)
            {
                Debug.WriteLine($"StartDrag CancelDrag");
                CancelDrag();
            }
            if (CurrentDrag?.Button == button)
            {
                Debug.WriteLine($"StartDrag Same drag");
                return;
            }

            Debug.WriteLine($"StartDrag remove click");

            CurrentDrag = new DragInfo
            {
                Source = source,
                Button = button
            };
            if (button == MouseButton.Right)
            {
                CurrentDrag.Operation = source.StartDragAsync(pointer);
            }    
            Debug.WriteLine($"StartDrag Current Drag set");
        }

        static void CancelDrag()
        {
            Debug.WriteLine($"CancelDrag pre lock");
            Debug.WriteLine($"CancelDrag");
            if (CurrentDrag == null)
            {
                return;
            }

            var drag = CurrentDrag;
            CurrentDrag = null;
            if (drag.Operation.Status == AsyncStatus.Started)
            {
                drag.Operation.Cancel();
            }
            CurrentDrag = null;
        }

        static void OnPointerWheelChanged(UIElement uiElement, PointerRoutedEventArgs winArgs)
        {
            Debug.WriteLine($"OnPointerWheelChanged");
            if (ClickCallbacks.TryGetValue(uiElement, out var sourceCallback) && sourceCallback.OnScroll != null)
            {
                Debug.WriteLine($"OnPointerWheelChanged has callback");
                var delta = winArgs.GetCurrentPoint(uiElement).Properties.MouseWheelDelta;
                var dir = Math.Sign(delta);
                sourceCallback.OnScroll(uiElement, dir);
                winArgs.Handled = true;
            }
            Debug.WriteLine($"OnPointerWheelChanged Done");
        }

        static void OnPointerReleased(object s, PointerRoutedEventArgs winArgs)
        {
            Debug.WriteLine($"OnPointerReleased");
            if (s is UIElement sender)
            {
                var pointer = winArgs.GetCurrentPoint((UIElement)winArgs.OriginalSource);
                var senderPointer = winArgs.GetCurrentPoint(sender);
                var props = pointer.Properties;
                MouseButton clickedButton;
                switch (props.PointerUpdateKind)
                {
                    case PointerUpdateKind.LeftButtonReleased:
                        clickedButton = MouseButton.Left;
                        break;
                    case PointerUpdateKind.RightButtonReleased:
                        clickedButton = MouseButton.Right;
                        if (pointer.Properties.IsRightButtonPressed)
                        {
                            return;
                        }
                        break;
                    case PointerUpdateKind.MiddleButtonReleased:
                        clickedButton = MouseButton.Middle;
                        break;
                    default:
                        return;
                }
                if (TrackedEvents.TryGetValue(clickedButton, out var clickStartedElement))
                {
                    TrackedEvents.Remove(clickedButton);
                    if (ClickCallbacks.TryGetValue(clickStartedElement, out var sourceCallback) &&
                        ClickCallbacks.TryGetValue(sender, out var destinationCallback))
                    {
                        if (clickStartedElement == sender)
                        {
                            if (sourceCallback?.OnClick != null)
                            {
                                sourceCallback.OnClick(sender, clickedButton);
                                winArgs.Handled = true;
                            }
                        }
                    }
                }
            }
        }

        private static void OnDropCompleted(UIElement dest, UIElement s, DropCompletedEventArgs e)
        {
            var drag = CurrentDrag;
            CurrentDrag = null;
            if (drag?.CurrentTarget == null)
            {
                return;
            }
            var source = drag.Source;
            var target = drag.CurrentTarget;
            var button = drag.Button;

            if (ClickCallbacks.TryGetValue(source, out var sourceCallback) && ClickCallbacks.TryGetValue(target, out var destinationCallback))
            {
                if (sourceCallback?.OnDragHintControlCompleted != null && destinationCallback?.OnDropHintControlCompleted != null)
                {
                    var dragged = sourceCallback.OnDragHintControlCompleted(source, drag.Button);
                    destinationCallback.OnDropHintControlCompleted(target, drag.Button, dragged);
                }

                if (sourceCallback?.OnDragImageCompleted != null && destinationCallback?.OnDropImageCompleted != null)
                {
                    var dragged = sourceCallback.OnDragImageCompleted(source, drag.Button);
                    destinationCallback.OnDropImageCompleted(target, drag.Button, dragged);
                }
            }
        }

    }
}
