using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace ChecklistTracker.ViewModel
{
    internal class TextParams
    {
        internal double FontSize { get; set; }
        internal Color FontColor { get; set; }
        public Brush FontColorBrush { get { return new SolidColorBrush(FontColor); } }
        internal Color BackgroundColor { get; set; }
        public Brush BackgroundColorBrush { get { return new SolidColorBrush(BackgroundColor); } }
    }
}
