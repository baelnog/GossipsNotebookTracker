using HPPH;
#if ENABLE_OCR
using HPPH.SkiaSharp;
using SkiaSharp;
#endif
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace ChecklistTracker.Images;

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

#if ENABLE_OCR
    public static async Task<SKBitmap> ToSKBitmapAsync(this SoftwareBitmap softwareBitmap)
    {
        using var stream = new InMemoryRandomAccessStream();
        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
        encoder.SetSoftwareBitmap(softwareBitmap);
        await encoder.FlushAsync();
        stream.Seek(0);
        return SKBitmap.Decode(stream.AsStreamForRead());
    }

    public static SKBitmap ToSKBitmap(this SoftwareBitmap softwareBitmap)
    {
        return softwareBitmap.ToSKBitmapAsync().GetAwaiter().GetResult();
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
#endif
}
