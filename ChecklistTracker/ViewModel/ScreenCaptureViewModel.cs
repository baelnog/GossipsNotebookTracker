using ChecklistTracker.CoreUtils;
using CommunityToolkit.WinUI.Collections;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using ScreenCapturerNS;
using SharpHook;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using DispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue;
using DispatcherQueuePriority = Microsoft.UI.Dispatching.DispatcherQueuePriority;

namespace ChecklistTracker.ViewModel
{
    public class ScreenCaptureViewModel
    {
        private ObservableCollection<ScreenCapture> _Screenshots { get; set; }

        public AdvancedCollectionView Screenshots { get; set; }
        public LayoutParams LayoutParams { get; set; }
        private DispatcherQueue DispatchQueue;

        private int ScreenIndex;
        private int GraphicsCardIndex;
        private Rectangle ClipRegion;
        private Bitmap? Latest = null;
        private object Lock = new();

        internal ScreenCaptureViewModel(int graphicsCardIndex, int screenIndex, Rectangle clipRegion, LayoutParams layout, TaskPoolGlobalHook globalHooks, DispatcherQueue dispatchQueue)
        {
            _Screenshots = new ObservableCollection<ScreenCapture>();
            Screenshots = new AdvancedCollectionView(_Screenshots);
            Screenshots.SortDescriptions.Add(new SortDescription("Index", SortDirection.Descending));
            LayoutParams = layout;
            DispatchQueue = dispatchQueue;
            GraphicsCardIndex = graphicsCardIndex;
            ScreenIndex = screenIndex;
            ClipRegion = clipRegion;

            globalHooks.KeyPressed += (o, evt) =>
            {
                if (evt.RawEvent.Keyboard.KeyCode == SharpHook.Data.KeyCode.VcLeftControl ||
                    evt.RawEvent.Keyboard.KeyCode == SharpHook.Data.KeyCode.VcRightControl)
                {
                    dispatchQueue.TryEnqueue(DispatcherQueuePriority.High, () => CaptureScreenshot());
                }
            };
        }

        private async void SaveScreenshotAsync(Bitmap bitmap)
        {
            Logging.WriteLine("SaveScreenshotAsync");
            using (bitmap)
            using (var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream())
            {
                bitmap.Save(stream.AsStream(), ImageFormat.Bmp);//choose the specific image format by your own bitmap source
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                var transform = new BitmapTransform();
                var width = 0.01 * bitmap.Width;
                var height = 0.01 * bitmap.Height;
                transform.Bounds = new BitmapBounds((uint)(ClipRegion.Left * width), (uint)(ClipRegion.Top * height), (uint)(ClipRegion.Width * width), (uint)(ClipRegion.Height * height));
                SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, transform, ExifOrientationMode.IgnoreExifOrientation, ColorManagementMode.DoNotColorManage);

                DispatchQueue.TryEnqueue(DispatcherQueuePriority.High, () =>
                {
                    var source = new SoftwareBitmapSource();
                    source.SetBitmapAsync(softwareBitmap).GetAwaiter().OnCompleted(() =>
                    {
                        Logging.WriteLine("SaveScreenshotAsync Add Screenshot");
                        _Screenshots.Add(new ScreenCapture(source, LayoutParams));
                    });
                    
                });
            }
        }

        internal void CaptureScreenshot()
        {
            Logging.WriteLine("Collect screenshot start");
            ScreenCapturer.SkipFirstFrame = true;
            ScreenCapturer.PreserveBitmap = true;
            var once = true;
            ScreenCapturer.StartCapture((bitmap) =>
            {
                if (!once) return;
                once = false;

                Logging.WriteLine("Collect screenshot collected");
                ScreenCapturer.StopCapture();
                SaveScreenshotAsync(bitmap);
            }, ScreenIndex, GraphicsCardIndex);
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
