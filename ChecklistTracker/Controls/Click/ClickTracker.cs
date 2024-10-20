using ChecklistTracker.CoreUtils;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ChecklistTracker.Controls.Click
{
    internal static partial class ClickTracker
    {

        private static Dictionary<MouseButton, UIElement> TrackedEvents = new Dictionary<MouseButton, UIElement>();

        private static DragInfo? CurrentDrag = null;

        private static Dictionary<UIElement, ClickCallbacks> ClickCallbacks = new Dictionary<UIElement, ClickCallbacks>();
        private static ConcurrentQueue<object> operations = new ConcurrentQueue<object>();

        internal static void ConfigureClickHandler(this UIElement source, ClickCallbacks callbacks)
        {
            ClickCallbacks[source] = callbacks;

            source.CanDrag = callbacks.OnDragImageCompleted != null || callbacks.OnDragHintControlCompleted != null;
            source.AllowDrop = callbacks.OnDropImageCompleted != null || callbacks.OnDropHintControlCompleted != null;
            source.DropCompleted += (s, e) => OnDropCompleted(source, s, e);
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
            var now = DateTime.Now;
            var sincelastRelease = now - LastRelease;
            if (sincelastRelease.TotalMilliseconds <= 50)
            {
                Logging.WriteLine($"Ignoring pointer press too close to release. Ignoring event. LastRelease {LastRelease}. Now {now}");
                return;
            }
            else
            {
                Logging.WriteLine($"Since last press {sincelastRelease.TotalMilliseconds}. LastRelease {LastRelease}. Now {now}");
            }
            if (s is UIElement sender)
            {
                Logging.WriteLine($"OnPointerPressed {sender}");
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
            Logging.WriteLine($"OnPointerMoved {s}");
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
                    e.Handled = true;
                }
                else
                {
                    return;
                }
                Logging.WriteLine($"OnPointerMoved {button}");
                Logging.WriteLine($"StartDrag {button}");
                StartDrag(button, sender, pointer);
            }
        }

        static void OnDragOver(object s, DragEventArgs e)
        {
            e.DragUIOverride.IsGlyphVisible = false;
            if (s is UIElement sender)
            {
                var drag = CurrentDrag;
                if (drag != null)
                {
                    Logging.WriteLine($"OnDragOver {drag}.CurrentTarget = {sender}");
                    drag.CurrentTarget = sender;
                }
                e.Handled |= true;
            }
        }

        static int closeCounter = 0;

        static void StartDrag(MouseButton button, UIElement source, PointerPoint pointer)
        {
            Logging.WriteLine($"StartDrag pre lock");
            Logging.WriteLine($"StartDrag");
            TrackedEvents.Remove(button);
            if (CurrentDrag?.Button != button)
            {
                Logging.WriteLine($"StartDrag CancelDrag");
                CancelDrag();
            }
            if (CurrentDrag?.Button == button)
            {
                Logging.WriteLine($"StartDrag Same drag");
                return;
            }

            Logging.WriteLine($"StartDrag remove click");

            var drag = new DragInfo
            {
                Source = source,
                Button = button
            };
            CurrentDrag = drag;
            Logging.WriteLine($"StartDrag Current Drag set");
        }

        static void CancelDrag()
        {
            Logging.WriteLine($"CancelDrag pre lock");
            Logging.WriteLine($"CancelDrag");
            if (CurrentDrag == null)
            {
                return;
            }

            var drag = CurrentDrag;
            CurrentDrag = null;
        }

        static void OnPointerWheelChanged(UIElement uiElement, PointerRoutedEventArgs winArgs)
        {
            Logging.WriteLine($"OnPointerWheelChanged");
            if (ClickCallbacks.TryGetValue(uiElement, out var sourceCallback) && sourceCallback.OnScroll != null)
            {
                Logging.WriteLine($"OnPointerWheelChanged has callback");
                var delta = winArgs.GetCurrentPoint(uiElement).Properties.MouseWheelDelta;
                var dir = Math.Sign(delta);
                sourceCallback.OnScroll(uiElement, dir);
                winArgs.Handled = true;
            }
            Logging.WriteLine($"OnPointerWheelChanged Done");
        }

        private static DateTime LastRelease = DateTime.MinValue;

        static void OnPointerReleased(object s, PointerRoutedEventArgs winArgs)
        {
            var last = LastRelease;
            LastRelease = DateTime.Now;
            Logging.WriteLine($"OnPointerReleased {s}. Last Release {last}. Now {LastRelease}.");
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
                if (clickedButton != MouseButton.Left && CurrentDrag?.Button == clickedButton)
                {
                    var drag = CurrentDrag;
                    var source = drag.Source;
                    var target = sender;
                    if (ClickCallbacks.TryGetValue(source, out var sourceCallback) && ClickCallbacks.TryGetValue(target, out var destinationCallback))
                    {
                        if (sourceCallback?.OnDragHintControlCompleted != null && destinationCallback?.OnDropHintControlCompleted != null)
                        {
                            var dragged = sourceCallback.OnDragHintControlCompleted(source, drag.Button);
                            destinationCallback.OnDropHintControlCompleted(target, drag.Button, dragged);
                            CurrentDrag = null;
                            winArgs.Handled = true;
                        }

                        if (sourceCallback?.OnDragImageCompleted != null && destinationCallback?.OnDropImageCompleted != null)
                        {
                            var dragged = sourceCallback.OnDragImageCompleted(source, drag.Button);
                            destinationCallback.OnDropImageCompleted(target, drag.Button, dragged);
                            CurrentDrag = null;
                            winArgs.Handled = true;
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
