using ChecklistTracker.CoreUtils;
using CommunityToolkit.WinUI.Collections;
using HPPH;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using ScreenCapture.NET;
using SharpHook;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Rectangle = System.Drawing.Rectangle;

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

        /// <summary>
        /// Adjusts the contrast of a SoftwareBitmap (Gray8 or BGRA8) using the algorithm described at:
        /// https://www.dfstudios.co.uk/articles/programming/image-programming-algorithms/image-processing-algorithms-part-5-contrast-adjustment/
        /// </summary>
        /// <param name="input">Input SoftwareBitmap (Gray8 or BGRA8)</param>
        /// <param name="contrast">Contrast value in range [-100, 100], 0 = no change</param>
        /// <returns>New SoftwareBitmap with adjusted contrast</returns>
        public static SoftwareBitmap AdjustContrast(SoftwareBitmap input, int contrast)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Clamp contrast to [-100, 100]
            contrast = Math.Max(-128, Math.Min(11, contrast));

            // Calculate contrast factor
            double factor = (259.0 * (contrast + 255)) / (255 * (259 - contrast));

            if (input.BitmapPixelFormat == BitmapPixelFormat.Gray8)
            {
                // Grayscale: adjust each pixel
                var buffer = new byte[input.PixelWidth * input.PixelHeight];
                input.CopyToBuffer(buffer.AsBuffer());

                for (int i = 0; i < buffer.Length; i++)
                {
                    int newValue = (int)(factor * (buffer[i] - 128) + 128);
                    buffer[i] = (byte)Math.Clamp(newValue, 0, 255);
                }

                var contrasted = new SoftwareBitmap(BitmapPixelFormat.Gray8, input.PixelWidth, input.PixelHeight, BitmapAlphaMode.Ignore);
                contrasted.CopyFromBuffer(buffer.AsBuffer());
                return contrasted;
            }
            else if (input.BitmapPixelFormat == BitmapPixelFormat.Bgra8)
            {
                // BGRA8: adjust R, G, B channels, leave A unchanged
                var buffer = new byte[input.PixelWidth * input.PixelHeight * 4];
                input.CopyToBuffer(buffer.AsBuffer());

                for (int i = 0; i < buffer.Length; i += 4)
                {
                    // B
                    int b = (int)(factor * (buffer[i + 0] - 128) + 128);
                    // G
                    int g = (int)(factor * (buffer[i + 1] - 128) + 128);
                    // R
                    int r = (int)(factor * (buffer[i + 2] - 128) + 128);
                    // A remains unchanged
                    buffer[i + 0] = (byte)Math.Clamp(b, 0, 255);
                    buffer[i + 1] = (byte)Math.Clamp(g, 0, 255);
                    buffer[i + 2] = (byte)Math.Clamp(r, 0, 255);
                    // buffer[i + 3] = buffer[i + 3];
                }

                var contrasted = new SoftwareBitmap(BitmapPixelFormat.Bgra8, input.PixelWidth, input.PixelHeight, input.BitmapAlphaMode);
                contrasted.CopyFromBuffer(buffer.AsBuffer());
                return contrasted;
            }
            else
            {
                // For other formats, convert to BGRA8 and process
                var converted = SoftwareBitmap.Convert(input, BitmapPixelFormat.Bgra8);
                return AdjustContrast(converted, contrast);
            }
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

            bitmap = SoftwareBitmap.Convert(bitmap, BitmapPixelFormat.Gray8);
            bitmap = AdjustContrast(bitmap, 96);

            var task = OcrHelper.RecognizeTextAsync(bitmap);
            task.GetAwaiter().OnCompleted(() =>
            {
                Logging.WriteLine($"OCR: {task.Result}");
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
