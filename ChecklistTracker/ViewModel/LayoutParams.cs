using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistTracker.ViewModel
{
    internal class LayoutParams
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
    }
}
