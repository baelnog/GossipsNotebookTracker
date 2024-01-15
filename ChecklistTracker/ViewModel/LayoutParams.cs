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

        public int Width { get; set; }
        public int Height { get; set; }
        public Thickness Padding { get; set; }

        internal LayoutParams(int width, int height, Thickness padding)
        {
            Width = width;
            Height = height;
            Padding = padding;
        }
    }
}
