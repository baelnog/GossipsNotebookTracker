using Microsoft.UI.Xaml;

namespace ChecklistTracker.ViewModel
{
    public class LayoutParams
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public Thickness Padding { get; set; }

        internal LayoutParams(double width, double height, Thickness padding)
        {
            Width = width;
            Height = height;
            Padding = padding;
        }

        internal LayoutParams(double width, double height) : this(width, height, new Thickness(0))
        {
        }
    }
}
