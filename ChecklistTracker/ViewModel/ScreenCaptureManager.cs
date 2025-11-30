using ChecklistTracker.Config;
using ChecklistTracker.CoreUtils;
using HPPH;
using HPPH.SkiaSharp;
using ScreenCapture.NET;
using SkiaSharp;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace ChecklistTracker.ViewModel
{
    internal partial class ScreenCaptureManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Display> AvailableDisplays { get; private set; }

        private Dictionary<int, Task<DX11ScreenCapture>> _ScreenCaptures = new ();
        private ConcurrentDictionary<(DX11ScreenCapture, Rectangle), CaptureZone<ColorBGRA>> _CaptureZones = new();

        private readonly DX11ScreenCaptureService ScreenCaptureService = new DX11ScreenCaptureService();
        private readonly UserConfig UserConfig;

        public int SelectedScreenIndex { get; set; }

        public ScreenCaptureManager(UserConfig userConfig)
        {
            this.UserConfig = userConfig;
            // Get all available graphics cards
            IEnumerable<GraphicsCard> graphicsCards = ScreenCaptureService.GetGraphicsCards();
            var graphicsCard = graphicsCards.FirstOrDefault();
            // Get the displays from the graphics card(s) you are interested in
            var displays = ScreenCaptureService.GetDisplays(graphicsCard).ToList();
            AvailableDisplays = new ObservableCollection<Display>(displays);

            foreach (var display in displays)
            {
                _ScreenCaptures.Add(display.Index, InitializeScreenCapture(display));
                if (display.DeviceName == userConfig.ScreenShotScreen)
                {
                    SelectedScreenIndex = display.Index;
                }
            }

            PropertyChanged += ScreenCaptureManager_PropertyChanged;
        }

        private void ScreenCaptureManager_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedScreenIndex))
            {
                UserConfig.ScreenShotScreen = AvailableDisplays
                    .Where(d => d.Index == SelectedScreenIndex)
                    .Select(d => d.DeviceName)
                    .FirstOrDefault();
            }
        }

        private async Task<DX11ScreenCapture> InitializeScreenCapture(Display display)
        {
            return await Task.Run(() => ScreenCaptureService.GetScreenCapture(display));
        }

        private SKBitmap? DoCapture(CaptureZone<ColorBGRA> captureZone)
        {
            using (captureZone.Lock())
            {
                var i0 = captureZone.Image[0, 0];

                if (!captureZone.RawBuffer.ContainsAnyExceptInRange((byte)0, (byte)0))
                {
                    return null;
                }

                return captureZone.Image.ToSKBitmap();
            }

        }

        public async Task<SKBitmap?> DoCapture(Rectangle clipRegion)
        {
            var screen = await _ScreenCaptures[SelectedScreenIndex];
            var captureZone = _CaptureZones.GetOrAdd((screen, clipRegion), (key) => RegisterCaptureZone(key.Item1, key.Item2));

            screen.CaptureScreen();

            var bitmap = DoCapture(captureZone);
            if (bitmap == null)
            {
                Logging.WriteLine("Capture failed, retrying...");
                bitmap = DoCapture(captureZone);
            }

            if (bitmap == null)
            {
                Logging.WriteLine("Capture failed again. Giving up...");
            }
            return bitmap;
        }

        private CaptureZone<ColorBGRA> RegisterCaptureZone(DX11ScreenCapture screen, Rectangle clipRegion)
        {
            var xFactor = 1.0 / screen.Display.Width;
            var yFactor = 1.0 / screen.Display.Height;

            var x = (int)(clipRegion.X * screen.Display.Width / 100.0);
            var y = (int)(clipRegion.Y * screen.Display.Height / 100.0);
            var width = (int)(clipRegion.Width * screen.Display.Width / 100.0);
            var height = (int)(clipRegion.Height * screen.Display.Height / 100.0);

            // Register the regions you want to capture on the screen
            // Capture the whole screen
            return screen.RegisterCaptureZone(x, y, width, height);
        }

    }
}
