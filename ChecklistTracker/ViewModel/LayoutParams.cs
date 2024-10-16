using Microsoft.UI.Xaml;

namespace ChecklistTracker.ViewModel
{
    public class LayoutParams
    {

        public int Width { get; set; }
        public int Height { get; set; }
        public Thickness Padding { get; set; }

        internal LayoutParams(int width, int height, Thickness padding)
        {
            Width = width;
            Height = height;
            Padding = padding;
        }

        internal LayoutParams(int width, int height) : this(width, height, new Thickness(0))
        {
        }
    }
}
