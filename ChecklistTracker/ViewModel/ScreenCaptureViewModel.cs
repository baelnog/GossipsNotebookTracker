using ChecklistTracker.Config;
using ChecklistTracker.Controls.Click;
using ChecklistTracker.CoreUtils;
using CommunityToolkit.WinUI.Collections;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using SharpHook;
using System.Collections.ObjectModel;
using Windows.Graphics.Imaging;
using Rectangle = System.Drawing.Rectangle;

namespace ChecklistTracker.ViewModel
{
    public class ScreenCaptureViewModel
    {
        private ObservableCollection<ScreenCapture> _Screenshots { get; set; }

        public AdvancedCollectionView Screenshots { get; set; }
        public LayoutParams LayoutParams { get; set; }

        private Rectangle ClipRegion;

        private ScreenCaptureManager ScreenCaptureManager;
        private DispatcherQueue Dispatcher;

        private TrackerConfig Config;

        internal ScreenCaptureViewModel(ScreenCaptureManager screenCaptureManager, Rectangle clipRegion, LayoutParams layout, TrackerConfig config, TaskPoolGlobalHook globalHooks, DispatcherQueue dispatchQueue)
        {
            Config = config;
            ScreenCaptureManager = screenCaptureManager;
            LayoutParams = layout;
            ClipRegion = clipRegion;
            Dispatcher = dispatchQueue;

            _Screenshots = new ObservableCollection<ScreenCapture>();
            Screenshots = new AdvancedCollectionView(_Screenshots);
            Screenshots.SortDescriptions.Add(new SortDescription("Index", SortDirection.Descending));

            globalHooks.KeyPressed += (o, evt) =>
            {
                if (Config.UserConfig.ScreenshotHotKeysEnabled && Config.UserConfig.ScreenshotKeys.Contains(evt.RawEvent.Keyboard.KeyCode))
                {
                    CaptureScreenshot();
                }
            };
        }

        internal void CaptureScreenshot()
        {
            ScreenCaptureManager.DoCapture(ClipRegion).ContinueWith(t =>
            {
                var bitmap = t.Result;
                if (bitmap == null)
                {
                    return;
                }

                Dispatcher.TryEnqueue(DispatcherQueuePriority.High, () => ProcessScreenShot(bitmap));
#if ENABLE_OCR
                var task = OcrHelper.RecognizeTextAsync(bitmap).ContinueWith(t =>
                {
                    Logging.WriteLine($"OCR: {t.Result}");
                });
#endif
            });
        }

        private void ProcessScreenShot(SoftwareBitmap softBitmap)
        {
            var source = new SoftwareBitmapSource();
            var addBitmapTask = source.SetBitmapAsync(softBitmap).AsTask().ContinueWith(_ =>
            {
                _Screenshots.Insert(0 ,new ScreenCapture(source, LayoutParams));
            });
        }

        internal void OnItemClick(UIElement sender, object item, MouseButton button)
        {
            if (button == MouseButton.Middle && item is ScreenCapture screenCapture)
            {
                _Screenshots.Remove(screenCapture);
            }
        }
    }

    internal class ScreenCapture
    {
        static int i = 0;
        public int Index { get; set; }
        internal ImageSource Screenshot { get; set; }
        internal LayoutParams LayoutParams { get; set; }

        internal ScreenCapture(ImageSource screenshot, LayoutParams layoutParams)
        {
            Screenshot = screenshot;
            LayoutParams = layoutParams;

            Index = i++;
        }
    }
}
