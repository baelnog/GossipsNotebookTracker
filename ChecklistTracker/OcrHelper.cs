using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Globalization;

namespace ChecklistTracker
{
    /// <summary>
    /// Provides OCR functionality using Windows.Media.Ocr for WinUI/.NET 8 apps.
    /// </summary>
    public static class OcrHelper
    {
        /// <summary>
        /// Recognizes text from a WriteableBitmap using Windows.Media.Ocr.
        /// </summary>
        /// <param name="bitmap">The WriteableBitmap to process.</param>
        /// <returns>The recognized text, or an empty string if OCR fails.</returns>
        public static async Task<string> RecognizeTextAsync(SoftwareBitmap softwareBitmap)
        {
            // Run OCR
            var ocrEngine = OcrEngine.TryCreateFromLanguage(new Language("en-us"));
            if (ocrEngine == null)
                throw new NotSupportedException("OCR engine could not be created. Check Windows language support.");

            var result = await ocrEngine.RecognizeAsync(softwareBitmap);
            return result?.Text ?? string.Empty;
        }
    }
}