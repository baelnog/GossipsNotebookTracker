using HPPH;
using HPPH.SkiaSharp;
using SkiaSharp;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace ChecklistTracker.Images
{
    public static class ImageProcessor
    {

        public static SoftwareBitmap ToSoftwareBitmap(this IImage image)
        {
            return image.ToRawArray().AsBuffer().ToSoftwareBitmap(image.Width, image.Height);
        }

        public static SoftwareBitmap ToSoftwareBitmap(this IBuffer image, int width, int height)
        {
            return SoftwareBitmap.CreateCopyFromBuffer(
                image,
                BitmapPixelFormat.Bgra8,
                width,
                height,
                BitmapAlphaMode.Premultiplied);
        }

        public static SoftwareBitmap ToSoftwareBitmap(this SKBitmap image)
        {
            return image.ToImage().ToSoftwareBitmap();
        }

        public static SKBitmap AdjustContrast(this SKBitmap image, float scale)
        {
            var newImage = new SKBitmap(image.Info);
            using var canvas = new SKCanvas(newImage);
            canvas.Clear(SKColors.White);
            using SKPaint paint = new SKPaint();
            paint.ColorFilter = SKColorFilter.CreateHighContrast(grayscale: false,
                contrast: scale,
                invertStyle: SKHighContrastConfigInvertStyle.NoInvert);

            canvas.DrawBitmap(image, 0, 0, paint);
            canvas.Flush();
            canvas.Save();
            return newImage;
        }
    }
}