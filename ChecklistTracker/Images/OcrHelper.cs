using ChecklistTracker.CoreUtils;
using ChecklistTracker.Images;
using Microsoft.UI.Xaml.Media.Imaging;
using SkiaSharp;
using System;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;

namespace ChecklistTracker
{
    /// <summary>
    /// Provides OCR functionality using Windows.Media.Ocr for WinUI/.NET 8 apps.
    /// </summary>
    public static class OcrHelper
    {
        /// <summary>
        /// Recognizes text from a SoftwareBitmap using Windows.Media.Ocr.
        /// </summary>
        /// <param name="softwareBitmap">The SoftwareBitmap to process.</param>
        /// <returns>The recognized text, or an empty string if OCR fails.</returns>
        public static async Task<string> RecognizeTextAsync(SKBitmap bitmap)
        {
            // Run OCR
            return await RecognizeTextAsync(bitmap.AdjustContrast(.75f).ToSoftwareBitmap());
        }

        /// <summary>
        /// Recognizes text from a SoftwareBitmap using Windows.Media.Ocr.
        /// </summary>
        /// <param name="softwareBitmap">The SoftwareBitmap to process.</param>
        /// <returns>The recognized text, or an empty string if OCR fails.</returns>
        public static async Task<string> RecognizeTextAsync(SoftwareBitmap softwareBitmap)
        {
            // Run OCR
            var ocrEngine = OcrEngine.TryCreateFromLanguage(new Language("en-us"));
            if (ocrEngine == null)
                throw new NotSupportedException("OCR engine could not be created. Check Windows language support.");

            var result = await ocrEngine.RecognizeAsync(softwareBitmap);
            Logging.WriteLine("OCR!");
            foreach (var line in result.Lines)
            {
                Logging.WriteLine($"  : {line.Text}");
            }
            Logging.WriteLine("OCR.");
            return result?.Text ?? string.Empty;
        }
    }
}