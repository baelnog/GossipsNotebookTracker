using ChecklistTracker.Config;
using ChecklistTracker.CoreUtils;
using ChecklistTracker.Images;
using CommunityToolkit.WinUI.Collections;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using SharpHook;
using SkiaSharp;
using System.Collections.ObjectModel;
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
                if (Config.UserConfig.ScreenshotKeys.Contains(evt.RawEvent.Keyboard.KeyCode))
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
                var task = OcrHelper.RecognizeTextAsync(bitmap).ContinueWith(t =>
                {
                    Logging.WriteLine($"OCR: {t.Result}");
                });
            });
        }

        private void ProcessScreenShot(SKBitmap bitmap)
        {
            var softBitmap = bitmap.ToSoftwareBitmap();
            var source = new SoftwareBitmapSource();
            var addBitmapTask = source.SetBitmapAsync(softBitmap).AsTask().ContinueWith(_ =>
            {
                _Screenshots.Add(new ScreenCapture(source, LayoutParams));
            });
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
