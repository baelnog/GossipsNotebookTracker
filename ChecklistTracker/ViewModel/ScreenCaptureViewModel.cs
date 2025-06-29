using CommunityToolkit.WinUI.Collections;
using HPPH;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using ScreenCapture.NET;
using SharpHook;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;

namespace ChecklistTracker.ViewModel
{
    public class ScreenCaptureViewModel
    {
        private ObservableCollection<ScreenCapture> _Screenshots { get; set; }

        public AdvancedCollectionView Screenshots { get; set; }
        public LayoutParams LayoutParams { get; set; }

        private int ScreenIndex;
        private int GraphicsCardIndex;
        private Rectangle ClipRegion;

        private static Lazy<DX11ScreenCaptureService> _ScreenCaptureService = new Lazy<DX11ScreenCaptureService>(() => new DX11ScreenCaptureService());
        private Lazy<DX11ScreenCapture> _ScreenCapture;

        private DX11ScreenCapture ScreenCapture { get => _ScreenCapture.Value; }


        internal ScreenCaptureViewModel(int graphicsCardIndex, int screenIndex, Rectangle clipRegion, LayoutParams layout, TaskPoolGlobalHook globalHooks, DispatcherQueue dispatchQueue)
        {
            _Screenshots = new ObservableCollection<ScreenCapture>();
            Screenshots = new AdvancedCollectionView(_Screenshots);
            Screenshots.SortDescriptions.Add(new SortDescription("Index", SortDirection.Descending));
            LayoutParams = layout;

            GraphicsCardIndex = graphicsCardIndex;
            ScreenIndex = screenIndex;
            ClipRegion = clipRegion;

            _ScreenCapture = new Lazy<DX11ScreenCapture>(() =>
            {

                // Get all available graphics cards
                IEnumerable<GraphicsCard> graphicsCards = _ScreenCaptureService.Value.GetGraphicsCards();

                // Get the displays from the graphics card(s) you are interested in
                IEnumerable<Display> displays = _ScreenCaptureService.Value.GetDisplays(graphicsCards.Skip(GraphicsCardIndex).First());

                // Create a screen-capture for all screens you want to capture
                DX11ScreenCapture sc = _ScreenCaptureService.Value.GetScreenCapture(displays.Skip(ScreenIndex).First());
                return sc;
            });

            globalHooks.KeyPressed += (o, evt) =>
            {
                if (evt.RawEvent.Keyboard.KeyCode == SharpHook.Data.KeyCode.VcLeftControl ||
                    evt.RawEvent.Keyboard.KeyCode == SharpHook.Data.KeyCode.VcRightControl)
                {
                    dispatchQueue.TryEnqueue(DispatcherQueuePriority.High, () => CaptureScreenshot());
                }
            };
        }

        internal void CaptureScreenshot()
        {
            var xFactor = 1.0 / ScreenCapture.Display.Width;
            var yFactor = 1.0 / ScreenCapture.Display.Height;

            var x = (int)(ClipRegion.X * ScreenCapture.Display.Width / 100.0);
            var y = (int)(ClipRegion.Y * ScreenCapture.Display.Height / 100.0);
            var width = (int)(ClipRegion.Width * ScreenCapture.Display.Width / 100.0);
            var height = (int)(ClipRegion.Height * ScreenCapture.Display.Height / 100.0);

            // Register the regions you want to capture on the screen
            // Capture the whole screen
            CaptureZone<ColorBGRA> captureZone = ScreenCapture.RegisterCaptureZone(x, y, width, height);

            ScreenCapture.CaptureScreen();

            var bitmap = new SoftwareBitmap(
                BitmapPixelFormat.Bgra8,
                width,
                height,
                BitmapAlphaMode.Premultiplied);

            var retry = false;
            using (captureZone.Lock())
            {
                var i0 = captureZone.Image[0, 0];
                retry = !captureZone.RawBuffer.ContainsAnyExceptInRange((byte)0, (byte)0);
                if (!retry)
                {
                    bitmap.CopyFromBuffer(captureZone.RawBuffer.ToArray().AsBuffer());
                }
            }
            if (retry)
            {
                CaptureScreenshot();
                return;
            }

            ScreenCapture.UnregisterCaptureZone(captureZone);
            var source = new SoftwareBitmapSource();
            source.SetBitmapAsync(bitmap).GetAwaiter().OnCompleted(() =>
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
